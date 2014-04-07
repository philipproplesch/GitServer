using System.Collections.Generic;
using devplex.GitServer.Core.Models;
using devplex.GitServer.Mvc.Common;

namespace devplex.GitServer.Mvc.ViewModels
{
    public class CommitDetailsViewModel : BaseRepositoryDetails
    {
        public CommitMessage CommitMessage { get; set; }
        public IEnumerable<FileDiff> Files { get; set; }
    }
}