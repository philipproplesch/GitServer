using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public class RepositoryDirectory : ITreeDirectory
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<ITreeDirectory> Directories { get; set; }

        public string PathWithoutExtension { get; set; }
        public List<string> Branches { get; set; }
    }
}