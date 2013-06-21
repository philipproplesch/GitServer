namespace devplex.GitServer.Core.Models
{
    public class FileDiff
    {
        public string Path { get; set; }
        public string Patch { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }
    }
}
