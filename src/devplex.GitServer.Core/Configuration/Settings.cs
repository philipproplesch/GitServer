using System;
using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    public class Settings
    {
        private static GitServerSection _section;
        public static GitServerSection Section
        {
            get
            {
                return _section ?? (_section = GitServerSection.GetSection());
            }
        }

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GitRoot
        {
            get
            {
                return GetValue("GitServer.GitRoot");
            }
        }

        public static bool UseSsl
        {
            get
            {
                return GetValue("GitServer.UseSSL").Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}