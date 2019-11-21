
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Dropbox.Api;
using Dropbox.Api.Files;
using TenantsApp.Shared.Interfaces;
using Xamarin.Forms;

namespace TenantsApp.Services
{
    public class DropBoxService : IDropboxService
    {
        #region Constants

        private const string dropboxFolder = "/";
        private const string AppKeyDropboxtoken = "";
        private const string ClientId = "";
        private const string RedirectUri = "https://www.google.com.au/";

        #endregion

        #region Fields



        private string oauth2State;

        #endregion

        #region Properties

        private string AccessToken { get; set; }
        public Action OnAuthenticated { get; set; }

        #endregion

        /// <summary>
        ///     <para>Runs the Dropbox OAuth authorization process if not yet authenticated.</para>
        ///     <para>Upon completion <seealso cref="OnAuthenticated"/> is called</para>
        /// </summary>
        /// <returns>An asynchronous task.</returns>
        public async Task Authorize()
        {
            if (!string.IsNullOrWhiteSpace(this.AccessToken))
            {
                // Already authorized
                this.OnAuthenticated?.Invoke();
                return;
            }

            if (this.LoadUserAccount())
            {
                // Found token and set AccessToken 
                return;
            }

            // Run Dropbox authentication
            this.oauth2State = Guid.NewGuid().ToString("N");
            var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, ClientId, new Uri(RedirectUri), this.oauth2State);
            var webView = new WebView { Source = new UrlWebViewSource { Url = authorizeUri.AbsoluteUri } };
            webView.Navigating += this.WebViewOnNavigating;
            ScrollView scrView = new ScrollView();
            scrView.Content = webView;
            var contentPage = new ContentPage { Content = scrView };
            await Application.Current.MainPage.Navigation.PushModalAsync(contentPage);
        }

        /// <summary>
        ///     Tries to find the Dropbox token in application settings
        /// </summary>
        /// <returns>Token as string or <c>null</c></returns>
        public bool LoadUserAccount()
        {
            try
            {
                if (!Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
                {
                    return false;
                }

                this.AccessToken = Application.Current.Properties[AppKeyDropboxtoken]?.ToString();
                if (this.AccessToken != null)
                {
                   
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Disconnect()
        {
            try
            {
                this.AccessToken = null;
                if (Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
                {
                    Application.Current.Properties.Remove(AppKeyDropboxtoken);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async void WebViewOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (!e.Url.StartsWith(RedirectUri, StringComparison.OrdinalIgnoreCase))
            {
                // we need to ignore all navigation that isn't to the redirect uri.
                return;
            }

            try
            {
                var result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(e.Url));

                if (result.State != this.oauth2State)
                {
                    return;
                }

                this.AccessToken = result.AccessToken;

                await SaveDropboxToken(this.AccessToken);
                this.OnAuthenticated?.Invoke();
            }
            catch (ArgumentException)
            {
                // There was an error in the URI passed to ParseTokenFragment
            }
            finally
            {
                e.Cancel = true;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        /// <summary>
        ///     Saves the Dropbox token to app settings
        /// </summary>
        /// <param name="token">Token received from Dropbox authentication</param>
        private static async Task SaveDropboxToken(string token)
        {
            if (token == null)
            {
                return;
            }

            try
            {
                Application.Current.Properties.Add(AppKeyDropboxtoken, token);
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Metadata

        public async Task<IList<Metadata>> ListFiles()
        {
            try
            {
                using (var client = this.GetClient())
                {
                    var list = await client.Files.ListFolderAsync(string.Empty);
                    // return list?.Entries;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<byte[]> ReadFile(string file)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    var response = await client.Files.DownloadAsync(dropboxFolder + file);
                    var bytes = response?.GetContentAsByteArrayAsync();
                    return bytes?.Result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public async Task<FileMetadata> WriteFile(byte[] fileContent, string filename)
        //{
        //    try
        //    {
        //        var commitInfo = new CommitInfo(filename, WriteMode.Overwrite.Instance, false, DateTime.Now);

        //        using (var client = this.GetClient())
        //        {
        //            var metadata = await client.Files.UploadAsync(commitInfo, new MemoryStream(fileContent));
        //            return metadata;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public async Task<bool> WriteFile(byte[] fileContent, string filename)
        {
           
                var commitInfo = new CommitInfo(dropboxFolder + filename, WriteMode.Overwrite.Instance, false, DateTime.Now);

                using (var client = this.GetClient())
                {
                    var metadata = await client.Files.UploadAsync(commitInfo, new MemoryStream(fileContent));
                    return true;
                }            
            
        }

        private DropboxClient GetClient()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return new DropboxClient(this.AccessToken, new DropboxClientConfig()
                {
                    HttpClient = new HttpClient(new
                       HttpClientHandler())
                });
            }
            return new DropboxClient(this.AccessToken);
        }
    }
}
