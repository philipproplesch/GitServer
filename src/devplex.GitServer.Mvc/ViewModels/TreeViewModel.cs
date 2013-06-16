using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class TreeViewModel
    {
        public string Branch { get; set; }
        public string RepositoryPath { get; set; }

        public RepositoryTree Tree { get; set; }
    }
}
