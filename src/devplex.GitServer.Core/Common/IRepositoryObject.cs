using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Common
{
    public interface IRepositoryObject : ITreeObject
    {
        ReducedCommit Commit { get; set; }
    }
}