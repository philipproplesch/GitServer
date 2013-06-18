using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    [ConfigurationCollection(
        typeof(ExtensionElement),
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ExtensionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionElement)element).Extension;
        }
    }
}