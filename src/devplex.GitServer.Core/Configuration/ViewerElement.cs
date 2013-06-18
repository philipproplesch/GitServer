using System;
using System.ComponentModel;
using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    public class ViewerElement : ConfigurationElement
    {
        private const string TypeName = "type";
        private const string ExtensionsName = "extensions";

        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(TypeName, IsRequired = true, IsKey = true)]
        public Type Type
        {
            get { return this[TypeName] as Type; }
        }
        
        [ConfigurationProperty(ExtensionsName, IsRequired = true)]
        public ExtensionCollection Extensions
        {
            get { return (ExtensionCollection)this[ExtensionsName]; }
        }
    }
}