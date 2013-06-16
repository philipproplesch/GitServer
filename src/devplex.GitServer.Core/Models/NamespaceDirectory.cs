using System.Collections.Generic;
using devplex.GitServer.Core.Common;

namespace devplex.GitServer.Core.Models
{
    public class NamespaceDirectory : ITreeObject
    {
        public int Order { get; private set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<ITreeObject> Objects { get; set; }
    }
}
