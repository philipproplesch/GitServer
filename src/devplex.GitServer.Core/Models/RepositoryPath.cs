using System;
using System.IO;
using System.Linq;
using devplex.GitServer.Core.Configuration;

namespace devplex.GitServer.Core.Models
{
    public class RepositoryPath
    {
        internal const string GitDirectory = ".git";

        public string RootPath { get; set; }
        public string AbsoluteRootPath { get; set; }

        public string RepositoryName { get; set; }

        public string SubPath { get; set; }

        public static RepositoryPath Resolve(string requestedPath)
        {
            var path = new RepositoryPath();

            if (requestedPath.EndsWith(GitDirectory))
            {
                path.RootPath = requestedPath;
                path.AbsoluteRootPath = GetAbsolutePath(requestedPath);
                path.RepositoryName = GetRepositoryName(requestedPath);

                return path;
            }

            var parts =
                requestedPath.Split(
                    new[] {".git/"},
                    StringSplitOptions.RemoveEmptyEntries);

            path.RootPath = parts[0] + ".git";
            path.AbsoluteRootPath = GetAbsolutePath(parts[0]);
            path.RepositoryName = GetRepositoryName(path.RootPath);

            if (parts.Length > 1)
            {
                path.SubPath = parts[1];
            }

            return path;
        }

        private static string GetAbsolutePath(string relativePath)
        {
            relativePath = relativePath.TrimStart('/', '\\');

            var root = Settings.GitRoot;

            return
                relativePath.EndsWith(GitDirectory)
                    ? Path.Combine(root, relativePath)
                    : Path.Combine(root, relativePath + GitDirectory);
        }

        private static string GetRepositoryName(string relativePath)
        {
            if (relativePath.EndsWith(GitDirectory))
            {
                var segments = relativePath.Split('/');
                var last = segments.Last();

                return last.Substring(0, last.Length - GitDirectory.Length);
            }

            return string.Empty;
        }
    }
}