using System.Collections.Generic;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class RepositoryViewModel
    {
        public string ReadMe { get; set; }
        public List<string> Branches { get; set; }

        public RepositoryPath Path { get; set; }
    }
}
