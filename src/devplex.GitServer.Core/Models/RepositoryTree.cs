﻿using System.Collections.Generic;
using devplex.GitServer.Core.Common;

namespace devplex.GitServer.Core.Models
{
    public class RepositoryTree
    {
        public List<IRepositoryObject> Directories { get; set; }
    }
}