using System.Collections.Generic;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class RepositoriesViewModel
    {
        public Dictionary<string, IEnumerable<OrganizationRepository>> Organizations { get; set; }
    }
}
