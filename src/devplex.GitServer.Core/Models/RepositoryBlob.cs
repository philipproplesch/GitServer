namespace devplex.GitServer.Core.Models
{
    public class RepositoryBlob
    {
        public string FileName { get; set; }
        
        public string Content { get; set; }
        public byte[] RawContent { get; set; }

        public string RawUrl { get; set; }

        public string FileSize { get; set; }

        public int Lines { get; set; }
        public int SourceLines { get; set; }

        public string Branch { get; set; }
        public string RepositoryPath { get; set; }
    }
}
