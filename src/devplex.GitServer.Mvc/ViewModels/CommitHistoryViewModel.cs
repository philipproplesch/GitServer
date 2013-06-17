using System.Collections.Generic;
using devplex.GitServer.Core.Models;
using devplex.GitServer.Mvc.Common;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class CommitHistoryViewModel : BaseRepositoryDetails
    {
        public IEnumerable<CommitMessage> Commits { get; set; }
    }
}
