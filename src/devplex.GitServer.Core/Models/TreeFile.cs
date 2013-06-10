using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public class TreeFile : ITreeObject
    {
        public int Order
        {
            get { return 2; }
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public List<ITreeObject> Objects { get; set; }
    }
}