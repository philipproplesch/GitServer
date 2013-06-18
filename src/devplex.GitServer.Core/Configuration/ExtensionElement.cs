using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    public class ExtensionElement : ConfigurationElement
    {
        private const string ExtensionName = "extension";

        [ConfigurationProperty(ExtensionName, IsRequired = true, IsKey = true)]
        public string Extension
        {
            get { return this[ExtensionName] as string; }
        }
    }
}