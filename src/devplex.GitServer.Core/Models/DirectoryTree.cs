using System.Collections.Generic;
using devplex.GitServer.Core.Common;

namespace devplex.GitServer.Core.Models
{
    public class DirectoryTree
    {
        public List<ITreeObject> Directories { get; set; }
    }
}