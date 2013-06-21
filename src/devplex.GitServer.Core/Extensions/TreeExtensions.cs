using System;
using System.IO;
using LibGit2Sharp;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Extensions
{
    public static class TreeExtensions
    {
        public static GitObject FindSubPath(this Tree instance, string subPath)
        {
            var segments =
                subPath.Split(
                    new[] { '/' },
                    StringSplitOptions.RemoveEmptyEntries);

            var obj = instance;
            foreach (var segment in segments)
            {
                foreach (var entry in obj)
                {
                    if (entry.Name.Equals(segment, StringComparison.OrdinalIgnoreCase))
                    {
                        if (entry.TargetType == TreeEntryTargetType.Tree)
                        {
                            obj = (Tree)entry.Target;
                            break;
                        }
                    }
                }
            }

            return obj;
        }

        public static RepositoryBlob FindFile(this Tree instance, string subPath)
        {
            var repositoryBlob = new RepositoryBlob();

            var segments =
                subPath.Split(
                    new[] { '/' },
                    StringSplitOptions.RemoveEmptyEntries);

            var obj = instance;
            foreach (var segment in segments)
            {
                foreach (var entry in obj)
                {
                    if (entry.Name.Equals(segment, StringComparison.OrdinalIgnoreCase))
                    {
                        if (entry.TargetType == TreeEntryTargetType.Tree)
                        {
                            obj = (Tree)entry.Target;
                            break;
                        }

                        if (entry.TargetType == TreeEntryTargetType.Blob)
                        {
                            var blob = (Blob)entry.Target;

                            repositoryBlob.FileName = entry.Name;
                            repositoryBlob.RawContent = blob.Content;

                            using (var ms = new MemoryStream(blob.Content))
                            using (var reader = new StreamReader(ms, true))
                            {
                                repositoryBlob.Content = reader.ReadToEnd();
                            }

                            repositoryBlob.FileSize =
                                string.Format(
                                    "{0:0.###} kb",
                                    ((double)blob.Content.LongLength / 1024));

                            return repositoryBlob;
                        }
                    }
                }
            }

            return null;
        }
    }
}