using System;

namespace devplex.GitServer.Core.Models
{
    public class ReducedCommit
    {
        public string Hash { get; set; }
        public string ShortHash { get; set; }
        public string Message { get; set; }
        public string CommitAuthor { get; set; }
        public DateTime? CommitDate { get; set; }
    }
}