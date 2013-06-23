using devplex.GitServer.Core.Models;
using devplex.GitServer.Mvc.Common;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class RepositoryTreeViewModel : BaseTreeViewModel<RepositoryTree>
    {
        public string ParentSubPath { get; set; }
    }
}