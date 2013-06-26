using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using LibGit2Sharp;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Extensions;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Versioning
{
    public class GitVersioningSystem : IVersioningSystem
    {
        private readonly string _branchName;

        private readonly Func<Repository, string, Branch> _getBranch =
            (repository, branchName)
            =>
            {
                Branch branch = null;
                if (!string.IsNullOrEmpty(branchName) && repository.Branches[branchName] != null)
                {
                    branch = repository.Branches[branchName];
                }
                else if (repository.Head != null)
                {
                    branch = repository.Head;
                }
                else if (repository.Branches.Any())
                {
                    branch = repository.Branches.FirstOrDefault();
                }

                return branch;
            };

        public GitVersioningSystem(string requestedPath, string branchName)
        {
            _branchName = branchName;
            Path = RepositoryPath.Resolve(requestedPath);
        }

        public GitVersioningSystem(string requestedPath)
            : this(requestedPath, string.Empty)
        { }

        public string SubPath
        {
            get { return Path.SubPath; }
        }

        public string RootPath
        {
            get { return Path.RootPath; }
        }

        public string AbsoluteRootPath
        {
            get { return Path.AbsoluteRootPath; }
        }

        private Repository Open()
        {
            return new Repository(Path.AbsoluteRootPath);
        }

        public RepositoryPath Path { get; set; }

        public IEnumerable<string> GetBranches()
        {
            using (var repository = Open())
            {
                return repository.Branches.Select(x => x.Name).ToList();
            }
        }

        public IEnumerable<CommitMessage> GetCommitMessages(
            int skip, int take)
        {
            var result = new List<CommitMessage>();

            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);

                result.AddRange(
                    branch.Commits.Select(
                        commit => new CommitMessage
                        {
                            Hash = commit.Sha,
                            ShortHash = commit.Sha,
                            Message = commit.Message,
                            AuthorName = commit.Author.Name,
                            AuthorMailAddress = commit.Author.Email,
                            Timestamp = commit.Author.When.UtcDateTime
                        }));
            }

            return result.Skip(skip).Take(take);
        }

        public IEnumerable<FileDiff> GetCommitDetails(string hash)
        {
            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);

                var commit =
                    branch.Commits
                          .FirstOrDefault(
                              x =>
                              x.Sha.Equals(
                                  hash,
                                  StringComparison.OrdinalIgnoreCase));

                if (commit == null)
                {
                    return null;
                }

                var parentCommit = commit.Parents.FirstOrDefault();

                var changes =
                    repository.Diff.Compare(
                        parentCommit != null ? parentCommit.Tree : null,
                        commit.Tree);

                if (changes != null)
                {
                    var result = new List<FileDiff>();
                    foreach (var change in changes)
                    {
                        var diff = new FileDiff
                        {
                            Path = change.Path,
                            Patch = change.Patch,
                            Additions = change.LinesAdded,
                            Deletions = change.LinesDeleted

                        };

                        result.Add(diff);
                    }

                    return result;
                }
            }

            return null;
        }

        public RepositoryTree GetRepositoryContent(bool includeCommitDetails)
        {
            var objects = new List<IRepositoryObject>();

            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);
                var commit = branch.Tip;

                var root = commit.Tree;
                if (!string.IsNullOrEmpty(Path.SubPath))
                {
                    root = root.FindSubPath(Path.SubPath) as Tree;
                    if (root == null)
                    {
                        return null;
                    }
                }

                foreach (var entry in root)
                {
                    IRepositoryObject obj;

                    // Directory
                    if (entry.Mode == Mode.Directory)
                    {
                        obj = new TreeDirectory();
                    }
                    // File
                    else
                    {
                        obj = new TreeFile();
                    }

                    obj.Name = entry.Name;
                    obj.Path = entry.Path.Replace('\\', '/');

                    if (includeCommitDetails)
                    {
                        // TODO: Get file/directory details!
                        // https://github.com/libgit2/libgit2sharp/issues/89

                        // TODO: Get short hash.

                        // =============================

                        //obj.Commit = new ReducedCommit {
                        //    Hash = commit.Sha,
                        //    ShortHash = commit.Id.ToString(),
                        //    Message = commit.MessageShort,
                        //    CommitAuthor = commit.Committer.Name,
                        //    CommitDate = commit.Committer.When.UtcDateTime,
                        //};

                        // =============================

                        //foreach (var actualCommit in branch.Commits)
                        //{
                        //    var parentCommit = actualCommit.Parents.FirstOrDefault();
                        //    if (parentCommit == null)
                        //    {
                        //        // Assume that the object was part of the initial commit.
                        //        obj.Commit = new ReducedCommit
                        //        {
                        //            Hash = actualCommit.Sha,
                        //            ShortHash = actualCommit.Sha,
                        //            Message = actualCommit.MessageShort,
                        //            CommitAuthor = actualCommit.Committer.Name,
                        //            CommitDate = actualCommit.Committer.When.UtcDateTime,
                        //        };

                        //        break;
                        //    }

                        //    var changes =
                        //        repository.Diff.Compare(
                        //            parentCommit.Tree,
                        //            actualCommit.Tree);

                        //    if (changes.Any(change => change.Path.Equals(entry.Path)))
                        //    {
                        //        obj.Commit = new ReducedCommit {
                        //            Hash = actualCommit.Sha,
                        //            ShortHash = actualCommit.Sha,
                        //            Message = actualCommit.MessageShort,
                        //            CommitAuthor = actualCommit.Committer.Name,
                        //            CommitDate = actualCommit.Committer.When.UtcDateTime,
                        //        };

                        //        break;
                        //    }
                        //}
                    }

                    objects.Add(obj);
                }
            }

            return new RepositoryTree { Directories = objects };
        }

        public RepositoryBlob GetBlobContent()
        {
            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);
                var commit = branch.Tip;

                var file = commit.Tree.FindFile(Path.SubPath);
                if (file == null)
                {
                    return null;
                }

                file.Branch = _branchName;
                file.RepositoryPath = Path.RootPath;

                return file;
            }
        }

        public string FindAndReadFile(
            Func<string, bool> filter)
        {
            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);
                var commit = branch.Tip;

                var root = commit.Tree;
                if (!string.IsNullOrEmpty(Path.SubPath))
                {
                    root = root.FindSubPath(Path.SubPath) as Tree;
                    if (root == null)
                    {
                        return null;
                    }
                }

                foreach (var entry in root)
                {
                    if (filter(entry.Name))
                    {
                        if (entry.TargetType != TreeEntryTargetType.Blob)
                        {
                            return string.Empty;
                        }

                        var blob = (Blob)entry.Target;

                        using (var ms = new MemoryStream(blob.Content))
                        using (var reader = new StreamReader(ms, true))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }

                return string.Empty;
            }
        }

        public ZipArchive GenerateZipArchive()
        {
            var now = DateTime.Now;

            Action<Tree, string, ZipOutputStream> compressTree = null;
            compressTree = (tree, path, output) =>
            {

                foreach (var child in tree)
                {
                    if (child.TargetType == TreeEntryTargetType.Tree)
                    {
                        var newPath = string.Concat(path, "/", child.Name);
                        compressTree(child.Target as Tree, newPath, output);
                    }
                    else if (child.TargetType == TreeEntryTargetType.Blob)
                    {
                        var blob = (Blob)child.Target;

                        var name =
                            string
                                .Concat(path, "/", child.Name)
                                .TrimStart('/');

                        var bytes = blob.Content;

                        var entry = new ZipEntry(name)
                        {
                            Size = bytes.Length,
                            DateTime = now
                        };

                        output.PutNextEntry(entry);

                        var buffer = new byte[4096];
                        using (var ms = new MemoryStream(bytes))
                        {
                            StreamUtils.Copy(ms, output, buffer);
                        }
                    }
                }
            };

            var archive = new ZipArchive
                {
                    Name =
                        string.Format(
                            "{0}-{1}.zip",
                            Path.RepositoryName,
                            _branchName)
                };

            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);
                var commit = branch.Tip;

                using (var ms = new MemoryStream())
                using (var zip = new ZipOutputStream(ms))
                {
                    zip.SetLevel(3);
                    compressTree(commit.Tree, string.Empty, zip);

                    zip.Finish();

                    ms.Position = 0;

                    archive.Data = ms.ToArray();
                }
            }

            return archive;
        }
    }
}
