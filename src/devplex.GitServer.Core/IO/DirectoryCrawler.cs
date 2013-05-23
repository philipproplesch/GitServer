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

        public BrowseDirectoryTree GetTree(string absolutePath)
        {
            return new BrowseDirectoryTree
            {
                Directories = GetDirectories(absolutePath)
            };
        }

        public List<BrowseDirectory> GetDirectories(string absolutePath, string relativePath = "")
        {
            var directories = new List<BrowseDirectory>();

            var rootDirectoryInfo = new DirectoryInfo(absolutePath);

            foreach (var directoryInfo in rootDirectoryInfo.EnumerateDirectories())
            {
                var directory = new BrowseDirectory();
                directory.Name = directoryInfo.Name;
                directory.Path = string.Concat(relativePath, "/", directoryInfo.Name);

                if (directoryInfo.Name.EndsWith(".git"))
                {
                    directory.Type = BrowseDirectoryType.Repository;
                }
                else
                {
                    directory.Type = BrowseDirectoryType.Directory;
                    directory.Directories = GetDirectories(directoryInfo.FullName, directory.Path);
                }

                directories.Add(directory);
            }

            return directories;
        }
    }
}