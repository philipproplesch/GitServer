using System;
using System.Collections.Generic;
using GitSharp;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Git
{
    public interface IGitRepository
    {
        string RootPath { get; }
        string AbsoluteRootPath { get; }

        Repository Init();
        Repository Open();

        IEnumerable<string> GetBranches();
        IEnumerable<CommitMessage> GetCommitMessages(int skip, int take);

        RepositoryTree GetRepositoryContent(bool includeCommitDetails);
        RepositoryBlob GetBlobContent();
        string FindAndReadFile(Func<string, bool> filter);

        ZipArchive GenerateZipArchive();
    }
}