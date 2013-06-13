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

            return path.EndsWith(".git") 
                ? Path.Combine(root, path)
                : Path.Combine(root, path + ".git");
        }

        public RepositoryPath SplitRepositoryPath(string path)
        {
            var result = new RepositoryPath();
            if (path.EndsWith(".git"))
            {
                result.RootPath = path;
                result.AbsoluteRootPath = GetRepositoryPath(path);

                return result;
            }

            var parts = path.Split(new[] {".git/"}, StringSplitOptions.RemoveEmptyEntries);

            result.RootPath = parts[0] + ".git";
            result.AbsoluteRootPath = GetRepositoryPath(parts[0]);

            if (parts.Length > 1)
            {
                result.SubPath = parts[1];
            }

            return result;
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

        public List<CommitMessage> GetCommitMessages(string branchName, RepositoryPath path)
        {
            var messages = new List<CommitMessage>();

            Action<Commit> addCommitMessage = commit =>
            {

                var commitMessage = new CommitMessage
                {
                    Hash = commit.Hash,
                    ShortHash = commit.ShortHash,
                    Message = commit.Message,
                    AuthorName = commit.Author.Name,
                    AuthorMailAddress = commit.Author.EmailAddress,
                    Timestamp = commit.CommitDate.UtcDateTime
                };

                messages.Add(commitMessage);
            };

            using (var repository = new Repository(path.AbsoluteRootPath))
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

        public DirectoryTree GetRepositoryContent(string branchName, RepositoryPath path)
        {
            var result = new DirectoryTree
            {
                Directories = new List<ITreeObject>()
            };
            
            using (var repository = new Repository(path.AbsoluteRootPath))
            {
                var branch = repository.Branches[branchName];
                var currentCommit = branch.CurrentCommit;

                var root = currentCommit.Tree;

                if (!string.IsNullOrEmpty(path.SubPath))
                {
                    var segments =
                        path.SubPath.Split(
                            new[] { "/" },
                            StringSplitOptions.RemoveEmptyEntries);

                    var newRoot = root;
                    foreach (var segment in segments)
                    {
                        var tree =
                            newRoot.Trees.FirstOrDefault(
                                child =>
                                child.Name.Equals(
                                    segment,
                                    StringComparison.Ordinal));

                        if (tree != null)
                        {
                            newRoot = tree;
                        }
                        else
                        {
                            return result;
                        }
                    }

                    root = newRoot;
                }

                foreach (var child in root.Children)
                {
                    // Is file?
                    if (child.IsBlob && child is Leaf)
                    {
                        var leaf = child as Leaf;
                        result.Directories.Add(
                            new TreeFile
                            {
                                Name = leaf.Name,
                                Path = leaf.Path
                            });
                    }
                    // Is directory?
                    else if (child is Tree)
                    {
                        var tree = child as Tree;
                        result.Directories.Add(
                            new TreeDirectory
                            {
                                Name = tree.Name,
                                Path = tree.Path
                            });
                    }
                }

                return result;
            }
        }

        public BlobDetails GetBlobContent(string branchName, RepositoryPath path)
        {
            var result = new BlobDetails();

            using (var repository = new Repository(path.AbsoluteRootPath))
            {
                var branch = repository.Branches[branchName];
                var currentCommit = branch.CurrentCommit;

                var root = currentCommit.Tree;

                var segments =
                        path.SubPath.Split(
                            new[] { "/" },
                            StringSplitOptions.RemoveEmptyEntries)
                               .ToList();

                var fileName = segments[segments.Count - 1];
                segments.RemoveAt(segments.Count - 1);

                var newRoot = root;
                foreach (var segment in segments)
                {
                    var tree =
                        newRoot.Trees.FirstOrDefault(
                            child =>
                            child.Name.Equals(
                                segment,
                                StringComparison.Ordinal));

                    if (tree != null)
                    {
                        newRoot = tree;
                    }
                }

                var file =
                    newRoot.Leaves.FirstOrDefault(
                        child =>
                        child.Name.Equals(
                            fileName,
                            StringComparison.OrdinalIgnoreCase));

                if (file != null)
                {
                    result.FileName = fileName;
                    //result.Content = file.Data;
                    result.RawContent = file.RawData;

                    using (var ms = new MemoryStream(file.RawData))
                    using (var reader = new StreamReader(ms, true))
                    {
                        result.Content = reader.ReadToEnd();
                    }

                    return result;
                }

                return null;
            }
        }

        public string FindFile(RepositoryPath path, Func<string, bool> filter)
        {
            using (var repository = new Repository(path.AbsoluteRootPath))
            {
                return FindFile(repository.CurrentBranch.Name, path, filter);
            }
        }

        public string FindFile(string branchName, RepositoryPath path, Func<string, bool> filter)
        {
            using (var repository = new Repository(path.AbsoluteRootPath))
            {
                var branch = repository.Branches[branchName];
                var currentCommit = branch.CurrentCommit;

                var root = currentCommit.Tree;

                if (!string.IsNullOrEmpty(path.SubPath))
                {
                    var segments =
                        path.SubPath.Split(
                            new[] { "/" },
                            StringSplitOptions.RemoveEmptyEntries)
                               .ToList();

                    var newRoot = root;
                    foreach (var segment in segments)
                    {
                        var tree =
                            newRoot.Trees.FirstOrDefault(
                                child =>
                                child.Name.Equals(
                                    segment,
                                    StringComparison.Ordinal));

                        if (tree != null)
                        {
                            newRoot = tree;
                        }
                    }

                    root = newRoot;
                }

                var file = root.Leaves.FirstOrDefault(x => filter(x.Name));
                return file != null ? file.Data : string.Empty;
            }
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
