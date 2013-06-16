using System.Collections.Generic;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class RepositoryViewModel
    {
        public string Path { get; set; }
        public string ReadMe { get; set; }
        public IEnumerable<string> Branches { get; set; }
    }
}
