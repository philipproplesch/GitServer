using System;
using System.ComponentModel;
using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    [ConfigurationCollection(typeof(ViewerElement), CollectionType = ConfigurationElementCollectionType.BasicMap, AddItemName = "viewer")]
    public class ViewerElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ViewerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ViewerElement)element).Type;
        }

        private const string FallbackViewerTypeName = "fallbackViewerType";
         [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(FallbackViewerTypeName, IsRequired = true)]
        public Type FallbackViewerType
        {
            get { return this[FallbackViewerTypeName] as Type; }
        }
    }

    public class ViewerElement : ConfigurationElement
    {
        private const string TypeName = "type";
        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(TypeName, IsRequired = true, IsKey = true)]
        public Type Type
        {
            get { return this[TypeName] as Type; }
        }

        private const string ExtensionsName = "extensions";
        [ConfigurationProperty(ExtensionsName, IsRequired = true)]
        public ViewerExtensionCollection Extensions
        {
            get { return (ViewerExtensionCollection)this[ExtensionsName]; }
        }
    }

    [ConfigurationCollection(typeof(ViewerExtensionElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ViewerExtensionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ViewerExtensionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ViewerExtensionElement)element).Extension;
        }
    }

    public class ViewerExtensionElement : ConfigurationElement
    {
        private const string ExtensionName = "extension";
        [ConfigurationProperty(ExtensionName, IsRequired = true, IsKey = true)]
        public string Extension
        {
            get { return this[ExtensionName] as string; }
        }
    }
}