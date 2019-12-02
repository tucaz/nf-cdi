using System.IO;
using NF.Hotmart;

namespace NF
{
    public class Global
    {
        public static Global Current;

        public Global()
        {
            Current = this;
        }

        public Configuration Hotmart { get; } = new Configuration();

        public static class Development
        {
            private static string _projectRootPath = @"D:\profile\workspace\backend-cdi\";
            
            static Development()
            {
                TempPath = Path.Combine(_projectRootPath, @"tools\temp");
                AppSettingsPath = Path.Combine(_projectRootPath, @"src\Backend\appsettings.json");
            }

            public static string TempPath;
            public static string AppSettingsPath;
        }    
    }
}