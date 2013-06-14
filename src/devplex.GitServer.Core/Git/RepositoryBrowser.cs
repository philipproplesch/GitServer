using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitSharp;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.Extensions;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Git
{
    public class RepositoryBrowser
    {
        public static string GetRepositoryPath(string path)
        {
            path = path.TrimStart('/', '\\');

            var root = Settings.GitRoot;

            return path.EndsWith(".git")
                ? Path.Combine(root, path)
                : Path.Combine(root, path + ".git");
        }

        public static string GetProjectName(string path)
        {
            const string suffix = ".git";

            if (path.EndsWith(suffix))
            {
                var segments = path.Split('/');
                var last = segments.Last();

                return last.Substring(0, last.Length - suffix.Length);
            }

            throw new Exception();
        }

        public RepositoryPath SplitRepositoryPath(string path)
        {
            var result = new RepositoryPath();
            if (path.EndsWith(".git"))
            {
                result.RootPath = path;
                result.AbsoluteRootPath = GetRepositoryPath(path);
                result.ProjectName = GetProjectName(path);

                return result;
            }

            var parts = path.Split(new[] { ".git/" }, StringSplitOptions.RemoveEmptyEntries);

            result.RootPath = parts[0] + ".git";
            result.AbsoluteRootPath = GetRepositoryPath(parts[0]);
            result.ProjectName = GetProjectName(result.RootPath);

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
                    root = root.FindSubPath(path.SubPath);

                    if (root == null)
                    {
                        return result;
                    }
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

                var file = currentCommit.Tree.FindFile(path.SubPath);
                if (file != null)
                {
                    result.FileName = file.Name;
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
                    root = root.FindSubPath(path.SubPath);
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

        public ZipArchive GenerateZipArchive(string branchName, RepositoryPath repositoryPath)
        {
            var now = DateTime.Now;

            Action<Tree, ZipOutputStream> compressTree = null;
            compressTree = (tree, output) =>
                {
                    var path = tree.Path;
                    foreach (var child in tree.Children)
                    {
                        if (child is Tree)
                        {
                            compressTree(child as Tree, output);
                        }
                        else if (child is Leaf)
                        {
                            var leaf = child as Leaf;
                            var name = string.Concat(path, "/", leaf.Name).TrimStart('/');

                            var bytes = leaf.RawData;

                            var entry = new ZipEntry(name);
                            entry.Size = bytes.Length;
                            //entry.DateTime = leaf.GetLastCommit().CommitDate.DateTime;
                            entry.DateTime = now;

                            output.PutNextEntry(entry);

                            var buffer = new byte[4096];
                            using (var ms = new MemoryStream(bytes))
                            {
                                StreamUtils.Copy(ms, output, buffer);
                            }
                        }
                    }
                };

            var archive = new ZipArchive();
            archive.Name =
                string.Format(
                    "{0}-{1}.zip",
                    repositoryPath.ProjectName,
                    branchName);

            using (var repository = new Repository(repositoryPath.AbsoluteRootPath))
            {
                var branch = repository.Branches[branchName];
                var currentCommit = branch.CurrentCommit;
                var root = currentCommit.Tree;

                using (var ms = new MemoryStream())
                using (var zip = new ZipOutputStream(ms))
                {
                    zip.SetLevel(3);
                    compressTree(root, zip);

                    zip.Finish();

                    ms.Position = 0;

                    archive.Data = ms.ToArray();
                }
            }

            return archive;
        }
    }
}
