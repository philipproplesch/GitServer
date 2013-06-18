using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    public class GitServerSection : ConfigurationSection
    {
        private const string SectionName = "devplex.GitServer";
        private const string UseSslName = "useSsl";
        private const string RepositoryPathName = "repositoryPath";
        private const string ViewersName = "viewers";
        private const string BinariesName = "binaries";

        public static GitServerSection GetSection()
        {
            return ConfigurationManager.GetSection(SectionName) as GitServerSection;
        }

        [ConfigurationProperty(UseSslName, DefaultValue = true)]
        public bool UseSsl
        {
            get
            {
                return (bool)this[UseSslName];
            }
        }

        [ConfigurationProperty(RepositoryPathName, IsRequired = true)]
        public string RepositoryPath
        {
            get
            {
                return (string)this[RepositoryPathName];
            }
        }

        [ConfigurationProperty(ViewersName)]
        public ViewerElementCollection Viewers
        {
            get
            {
                return (ViewerElementCollection)this[ViewersName];
            }
        }

        [ConfigurationProperty(BinariesName, IsRequired = true)]
        public ExtensionCollection Extensions
        {
            get { return (ExtensionCollection)this[BinariesName]; }
        }
    }
}
