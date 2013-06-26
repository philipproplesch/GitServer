using System;
using System.Collections.Generic;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Versioning
{
    public interface IVersioningSystem
    {
        RepositoryPath Path { get; set; }

        IEnumerable<string> GetBranches();
        IEnumerable<CommitMessage> GetCommitMessages(int skip, int take);
        IEnumerable<FileDiff> GetCommitDetails(string hash);

        RepositoryTree GetRepositoryContent(bool includeCommitDetails);
        RepositoryBlob GetBlobContent();
        string FindAndReadFile(Func<string, bool> filter);

        ZipArchive GenerateZipArchive();
    }
}