﻿using devplex.GitServer.Core.Common;

namespace devplex.GitServer.Core.Models
{
    public class TreeFile : IRepositoryObject
    {
        public int Order
        {
            get { return 2; }
        }

        public string Name { get; set; }
        public string Path { get; set; }

        public ReducedCommit Commit { get; set; }
    }
}