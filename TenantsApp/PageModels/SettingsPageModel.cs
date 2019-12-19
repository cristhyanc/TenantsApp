using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using TenantsApp.Entities.Interfaces;
using TenantsApp.Shared.Interfaces;
using Xamarin.Forms;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SettingsPageModel : BaseViewModel
    {

        public ICommand ConnectToDropboxCommand { get; set; }
        public ICommand DisconnectToDropboxCommand { get; set; }
        public ICommand BackupDatabaseCommand { get; set; }
        public ICommand RestoreDatabaseCommand { get; set; }
        public ICommand CleanDatabaseCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        
        public bool ConnectionButtonVisible { get; set; }
        public bool DisconnectionButtonVisible { get; set; }

        public string BillerName { get; set; }

        public string Version
        {
            get
            {
                var assembly = typeof(SettingsPageModel).GetTypeInfo().Assembly;
                var assemblyName = new AssemblyName(assembly.FullName);
                return assemblyName.Version.Major + "." + assemblyName.Version.Minor + "." + assemblyName.Version.Build;
            }
        }

        IDropboxService _dropbox;
        IUnitOfWork _unitOfWork;

        public SettingsPageModel(IUserDialogs userDialogs, IDropboxService dropbox, IUnitOfWork unitOfWork ) : base(userDialogs)
        {
            ConnectToDropboxCommand = new Command(Connect);
            DisconnectToDropboxCommand = new Command(Disconnect);
            BackupDatabaseCommand = new Command(BackupDb);
            RestoreDatabaseCommand = new Command(RestoreDB);
            CleanDatabaseCommand = new Command(CleanDB);
            SaveCommand = new Command(SaveSettings);
            this.BillerName = Application.Current.Properties["BILLERNAME"].ToString();
            _dropbox = dropbox;
            _unitOfWork = unitOfWork;
        }

        private async void SaveSettings()
        {
            try
            {
                this.IsBusy = true;
                Application.Current.Properties["BILLERNAME"] = this.BillerName;
                await Application.Current.SavePropertiesAsync();
                this.IsBusy = false;
                _userDialogs.Alert("Done");
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            try
            {
                base.ViewIsDisappearing(sender, e);
                _dropbox.OnAuthenticated = null;
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
           
            try
            {
                base.ViewIsAppearing(sender, e);
                LoadDropboxSettings();
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }

        private void LoadDropboxSettings()
        {
            try
            {
                
                _dropbox.OnAuthenticated = DropboxConnected;
                DisconnectionButtonVisible = false;
                this.ConnectionButtonVisible = false;
                if (_dropbox.LoadUserAccount())
                {
                    this.DisconnectionButtonVisible = true;
                }
                else
                {
                    this.ConnectionButtonVisible = true;
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }
       
        private async void Disconnect()
        {
            try
            {
               if( _dropbox.Disconnect())
                {
                    _userDialogs.Alert("Done");
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }

        private async void Connect()
        {
            try
            {  
                await _dropbox.Authorize();
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }

        private void DropboxConnected()
        {
            try
            {
                LoadDropboxSettings();
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
        }

        private async  void  BackupDb()
        {
            try
            {
                if (!await _userDialogs.ConfirmAsync("Do you want to backup the database?"))
                {
                    return;
                }

                this.IsBusy = true;
                var file = File.ReadAllBytes(TenantsApp.Shared.Helper.DBFilePath);
                if (file != null)
                {
                  if( await _dropbox.WriteFile(file, TenantsApp.Shared.Helper.DBFileName))
                    {
                        _userDialogs.Alert("Done");
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void RestoreDB()
        {
            try
            {

                if (!await _userDialogs.ConfirmAsync("Do you want to restore the database?"))
                {
                    return;
                }

                this.IsBusy = true;
                var file = await _dropbox.ReadFile(TenantsApp.Shared.Helper.DBFileName);

                if (file != null)
                {
                    File.WriteAllBytes(TenantsApp.Shared.Helper.DBFilePath, file);
                    _userDialogs.Alert("Done");
                    App.InitApp();
                }

            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async void CleanDB()
        {
            try
            {

                if (!await _userDialogs.ConfirmAsync("Do you want to Clean the database?"))
                {
                    return;
                }

                this.IsBusy = true;             
                File.WriteAllBytes(TenantsApp.Shared.Helper.DBFilePath, new byte[0]);               
                _userDialogs.Alert("Done");
                App.InitApp();
            }
            catch (Exception ex)
            {
                Helpers.ExceptionHelper.ProcessException(ex, _userDialogs, nameof(TenantsPageModel));
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
