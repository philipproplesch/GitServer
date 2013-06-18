using System.Configuration;

namespace devplex.GitServer.Core.Configuration
{
    public class GitServerSection : ConfigurationSection
    {
        private const string SectionName = "devplex.GitServer";
        public static GitServerSection GetSection()
        {
            return ConfigurationManager.GetSection(SectionName) as GitServerSection;
        }
        
        private const string ViewersName = "viewers";
        [ConfigurationProperty(ViewersName)]
        public ViewerElementCollection Viewers
        {
            get
            {
                return (ViewerElementCollection)this[ViewersName];
            }
        }
    }
}
