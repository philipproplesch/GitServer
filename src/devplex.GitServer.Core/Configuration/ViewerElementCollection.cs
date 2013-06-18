using System;
using System.ComponentModel;
using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    [ConfigurationCollection(
        typeof(ViewerElement),
        CollectionType = ConfigurationElementCollectionType.BasicMap,
        AddItemName = "viewer")]
    public class ViewerElementCollection : ConfigurationElementCollection
    {
        private const string FallbackViewerTypeName = "fallbackViewerType";

        protected override ConfigurationElement CreateNewElement()
        {
            return new ViewerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ViewerElement)element).Type;
        }

        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(FallbackViewerTypeName, IsRequired = true)]
        public Type FallbackViewerType
        {
            get { return this[FallbackViewerTypeName] as Type; }
        }
    }
}