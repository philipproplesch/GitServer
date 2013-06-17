using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitSharp;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Extensions;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Git
{
    public class GitRepository : IGitRepository
    {
        private readonly string _branchName;
        private readonly RepositoryPath _path;

        private readonly Func<Repository, string, Branch> _getBranch =
            (repository, branchName) =>
                {
                    if (repository.Branches.ContainsKey(branchName))
                    {
                        return repository.Branches[branchName];
                    }

                    if (repository.CurrentBranch != null)
                    {
                        return repository.CurrentBranch;
                    }

                    if (repository.Branches.Count > 0)
                    {
                        return repository.Branches.First().Value;
                    }

                    return null;
                };

        public GitRepository(string requestedPath, string branchName)
        {
            if (string.IsNullOrEmpty(branchName))
            {
                branchName = "master";
            }

            _branchName = branchName;
            _path = RepositoryPath.Resolve(requestedPath);
        }

        public GitRepository(string requestedPath)
            : this(requestedPath, string.Empty)
        { }

        public string RootPath
        {
            get { return _path.RootPath; }
        }

        public string AbsoluteRootPath
        {
            get { return _path.AbsoluteRootPath; }
        }

        public Repository Init()
        {
            return Repository.Init(_path.AbsoluteRootPath, true);
        }

        public Repository Open()
        {
            return new Repository(_path.AbsoluteRootPath);
        }

        public IEnumerable<string> GetBranches()
        {
            using (var repository = Open())
            {
                return repository.Branches.Select(x => x.Key).ToList();
            }
        }

        public IEnumerable<CommitMessage> GetCommitMessages(
            int skip, int take)
        {
            var result = new List<CommitMessage>();

            Action<List<CommitMessage>, Commit>
                addCommitMessage = (list, commit) =>
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

                        list.Add(commitMessage);
                    };

            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);

                var currentCommit = branch.CurrentCommit;
                addCommitMessage(result, currentCommit);

                foreach (var ancestor in currentCommit.Ancestors)
                {
                    addCommitMessage(result, ancestor);
                }
            }

            return result.Skip(skip).Take(take);
        }

        public RepositoryTree GetRepositoryContent()
        {
            var result = new RepositoryTree
            {
                Directories = new List<IRepositoryObject>()
            };

            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);
                var currentCommit = branch.CurrentCommit;
                
                var root = currentCommit.Tree;

                if (!string.IsNullOrEmpty(_path.SubPath))
                {
                    root = root.FindSubPath(_path.SubPath);

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
                        
                        var file = new TreeFile
                            {
                                Name = leaf.Name,
                                Path = leaf.Path
                            };

                        //var commit = leaf.GetLastCommit();
                        //if (commit != null)
                        //{
                        //    file.Message = commit.Message;
                        //    file.CommitDate = commit.CommitDate.UtcDateTime;
                        //}
                        //else
                        //{
                        //    file.CommitDate = currentCommit.CommitDate.UtcDateTime;
                        //}

                        result.Directories.Add(file);
                    }
                    // Is directory?
                    else if (child is Tree)
                    {
                        var tree = child as Tree;

                        var directory = new TreeDirectory
                            {
                                Name = tree.Name,
                                Path = tree.Path
                            };

                        //var commit = tree.GetLastCommit();
                        //if (commit != null)
                        //{
                        //    directory.Message = commit.Message;
                        //    directory.CommitDate = commit.CommitDate.UtcDateTime;
                        //}
                        //else
                        //{
                        //    directory.CommitDate = currentCommit.CommitDate.UtcDateTime;
                        //}

                        result.Directories.Add(directory);
                    }
                }

                return result;
            }
        }

        public RepositoryBlob GetBlobContent()
        {
            var result = new RepositoryBlob();

            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);
                var currentCommit = branch.CurrentCommit;

                var file = currentCommit.Tree.FindFile(_path.SubPath);
                if (file != null)
                {
                    result.FileName = file.Name;
                    result.RawContent = file.RawData;
                    //result.Content = file.Data;

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

        public string FindAndReadFile(
            Func<string, bool> filter)
        {
            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);

                var currentCommit = branch.CurrentCommit;

                var root = currentCommit.Tree;

                if (!string.IsNullOrEmpty(_path.SubPath))
                {
                    root = root.FindSubPath(_path.SubPath);
                }

                var file = root.Leaves.FirstOrDefault(x => filter(x.Name));

                return
                    file != null
                        ? file.Data
                        : string.Empty;
            }
        }

        public ZipArchive GenerateZipArchive()
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
                            _path.RepositoryName,
                            _branchName)
                };

            using (var repository = Open())
            {
                var branch = _getBranch(repository, _branchName);
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
