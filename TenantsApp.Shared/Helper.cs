using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TenantsApp.Shared
{
    public class Helper
    {
        public const  string  DBFileName = "TenantsAppDbV12.db";
        public static  string DBFilePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DBFileName);
            }
        }
    }
}
