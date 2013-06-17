using System.Collections.Generic;
using devplex.GitServer.Mvc.Common;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class RepositoryViewModel : BaseRepositoryDetails
    {
        public string ReadMe { get; set; }
        public IEnumerable<string> Branches { get; set; }
    }
}
