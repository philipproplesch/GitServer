using System;
using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    public class Settings
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
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