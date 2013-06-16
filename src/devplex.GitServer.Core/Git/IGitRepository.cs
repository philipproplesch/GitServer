using System;
using System.Collections.Generic;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Git
{
    public interface IGitRepository
    {
        string RootPath { get; }
        IEnumerable<string> GetBranches();
        IEnumerable<GitCommitMessage> GetCommitMessages();

        RepositoryTree GetRepositoryContent();
        RepositoryBlob GetBlobContent();
        string FindAndReadFile(Func<string, bool> filter);

        ZipArchive GenerateZipArchive();
    }
}