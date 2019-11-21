using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TenantsApp.Shared.Interfaces
{
   public interface IDropboxService
    {
        Task Authorize();
        Action OnAuthenticated { get; set; }
       // string AccessToken { get; set; }

        //Task<IList<object>> ListFiles();

        bool Disconnect();
        Task<byte[]> ReadFile(string file);
        Task<bool> WriteFile(byte[] fileContent, string filename);
        bool LoadUserAccount();
    }
}
