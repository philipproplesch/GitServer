using System;

namespace devplex.GitServer.Core.Models
{
    public class CommitMessage
    {
        public string Hash { get; set; }
        public string ShortHash { get; set; }
        public string Message { get; set; }
        public string ShortMessage { get; set; }
        public string AuthorName { get; set; }
        public string AuthorMailAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
