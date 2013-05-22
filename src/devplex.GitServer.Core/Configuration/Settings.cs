using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    public class Settings
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}