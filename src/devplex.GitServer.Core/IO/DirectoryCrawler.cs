using System;
using System.Collections.Generic;
using System.IO;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.IO
{
    public class DirectoryCrawler
    {
        public string GetAbsolutePath(string path)
        {
            var root = Settings.GetValue("GitServer.GitRoot");
            return Path.Combine(root, path);
        }

        public DirectoryTree GetTree(string absolutePath)
        {
            return new DirectoryTree
            {
                Directories = GetDirectories(absolutePath)
            };
        }

        public List<BrowseDirectory> GetDirectories(string absolutePath, string relativePath = "")
        {
            var directories = new List<BrowseDirectory>();

            var rootDirectoryInfo = new DirectoryInfo(absolutePath);

            try
            {
                foreach (var directoryInfo in rootDirectoryInfo.EnumerateDirectories())
                {
                    var directory = new BrowseDirectory();
                    directory.Name = directoryInfo.Name;

                    directory.Path =
                        string.Concat(
                            relativePath, "/", directoryInfo.Name);

                    if (directoryInfo.Name.EndsWith(".git"))
                    {
                        directory.Type = DirectoryType.Repository;

                        directory.PathWithoutExtension =
                            string.Concat(
                                relativePath,
                                "/",
                                directoryInfo.Name.Substring(0, directoryInfo.Name.Length - 4));
                    }
                    else
                    {
                        directory.Type = DirectoryType.Directory;

                        var subDirectories =
                            GetDirectories(
                                directoryInfo.FullName,
                                directory.Path);

                        if (subDirectories.Count == 0)
                        {
                            continue;
                        }

                        directory.Directories = subDirectories;
                    }

                    directories.Add(directory);
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