using System;

namespace devplex.GitServer.Core.Common
{
    public interface IRepositoryObject : ITreeObject
    {
        string Message { get; set; }
        DateTime? CommitDate { get; set; }
    }
}