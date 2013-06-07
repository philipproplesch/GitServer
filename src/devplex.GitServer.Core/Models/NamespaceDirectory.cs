using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public class NamespaceDirectory : ITreeDirectory
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<ITreeDirectory> Directories { get; set; }
    }
}
