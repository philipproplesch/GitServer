using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitSharp;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Git
{
    public class RepositoryBrowser
    {
        public static string GetRepositoryPath(string path)
        {
            path = path.TrimStart('/', '\\');

            var root = Settings.GetValue("GitServer.GitRoot");
            return Path.Combine(root, path + ".git");
        }

        public string GetLatestCommitMessage(string branchName, string path)
        {
            using (var repository = new Repository(path))
            {
                if (!repository.Branches.Any())
                {
                    return string.Empty;
                }

                var branch =
                    repository.Branches.ContainsKey("master")
                        ? repository.Branches["master"]
                        : repository.Branches.First().Value;

                if (branch != null && branch.CurrentCommit != null)
                {
                    return branch.CurrentCommit.Message;
                }
            }

            return string.Empty;
        }

        public List<CommitMessage> GetCommitMessages(string branchName, string path)
        {
            var messages = new List<CommitMessage>();

            Action<Commit> addCommitMessage = commit => {

                var commitMessage = new CommitMessage {
                    Hash = commit.Hash,
                    ShortHash = commit.ShortHash,
                    Message = commit.Message,
                    AuthorName = commit.Author.Name,
                    AuthorMailAddress = commit.Author.EmailAddress,
                    Timestamp = commit.CommitDate.DateTime
                };

                messages.Add(commitMessage);
            };
            
            var absolutePath = GetRepositoryPath(path);
            using (var repository = new Repository(absolutePath))
            {
                var branch = repository.Branches[branchName];

                var currentCommit = branch.CurrentCommit;
                addCommitMessage(currentCommit);

                foreach (var ancestor in currentCommit.Ancestors)
                {
                    addCommitMessage(ancestor);
                }
            }

            return messages;
        }

        public List<string> GetBranches(string path)
        {
            using (var repository = new Repository(path))
            {
                return repository.Branches.Select(x => x.Key).ToList();
            }
        }
    }
}
