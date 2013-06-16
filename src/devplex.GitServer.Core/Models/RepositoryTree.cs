using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public class RepositoryTree
    {
        public List<ITreeObject> Directories { get; set; }
    }
}