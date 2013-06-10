using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public class TreeDirectory : ITreeObject
    {
        public int Order
        {
            get { return 1; }
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public List<ITreeObject> Objects { get; set; }
    }
}