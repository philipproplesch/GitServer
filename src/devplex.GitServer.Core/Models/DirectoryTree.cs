using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public class DirectoryTree
    {
        public List<ITreeObject> Directories { get; set; }
    }
}