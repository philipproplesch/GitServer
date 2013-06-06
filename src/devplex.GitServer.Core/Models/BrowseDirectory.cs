﻿using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public class BrowseDirectory
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string PathWithoutExtension { get; set; }

        public string Message { get; set; }

        public BrowseDirectoryType Type { get; set; }
        public List<BrowseDirectory> Directories { get; set; }
    }
}