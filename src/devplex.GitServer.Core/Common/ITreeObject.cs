namespace devplex.GitServer.Core.Common
{
    public interface ITreeObject
    {
        int Order { get; }

        string Name { get; set; }
        string Path { get; set; }
    }
}
