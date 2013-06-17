namespace devplex.GitServer.Core.Models
{
    public class RepositoryBlob
    {
        public string FileName { get; set; }
        
        public string Content { get; set; }
        public byte[] RawContent { get; set; }

        public string RawUrl { get; set; }

        public string Branch { get; set; }
        public string RepositoryPath { get; set; }
    }
}
