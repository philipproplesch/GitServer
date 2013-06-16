using System;
using System.Collections.Generic;
using devplex.GitServer.Core.Common;

namespace devplex.GitServer.Core.Models
{
    public class TreeDirectory : IRepositoryObject
    {
        public int Order
        {
            get { return 1; }
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public List<ITreeObject> Objects { get; set; }

        public string Message { get; set; }
        public DateTime CommitDate { get; set; }
    }
}