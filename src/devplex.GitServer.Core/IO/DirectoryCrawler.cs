using System;
using System.Collections.Generic;
using System.IO;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.Git;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.IO
{
    public class DirectoryCrawler
    {
        private RepositoryBrowser _repositoryBrowser;

        public string GetAbsolutePath(string path)
        {
            var root = Settings.GetValue("GitServer.GitRoot");
            return Path.Combine(root, path);
        }

        public DirectoryTree GetTree(string absolutePath)
        {
            _repositoryBrowser = new RepositoryBrowser();

            return new DirectoryTree
            {
                Directories = GetDirectories(absolutePath)
            };
        }

        public List<ITreeObject> GetDirectories(string absolutePath, string relativePath = "")
        {
            var directories = new List<ITreeObject>();

            var rootDirectoryInfo = new DirectoryInfo(absolutePath);

            try
            {
                foreach (var directoryInfo in rootDirectoryInfo.EnumerateDirectories())
                {
                    var name = directoryInfo.Name;
                    var path = string.Concat(relativePath, "/", name);

                    if (directoryInfo.Name.EndsWith(".git"))
                    {
                        var directory = new RepositoryDirectory
                        {
                            Name = name,
                            Path = path,
                            PathWithoutExtension =
                                string.Concat(
                                    relativePath,
                                    "/",
                                    directoryInfo.Name.Substring(
                                        0, directoryInfo.Name.Length - 4)),
                            Branches =
                                _repositoryBrowser.GetBranches(
                                    directoryInfo.FullName)
                        };

                        directories.Add(directory);
                    }
                    else
                    {
                        var directory =
                            new NamespaceDirectory
                            {
                                Name = name,
                                Path = path
                            };

                        var subDirectories =
                            GetDirectories(
                                directoryInfo.FullName,
                                directory.Path);

                        if (subDirectories.Count == 0)
                        {
                            continue;
                        }

                        directory.Objects = subDirectories;

                        directories.Add(directory);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                //
            }

            return directories;
        }
    }
}