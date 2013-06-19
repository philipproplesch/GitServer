using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.IO
{
    public class DirectoryCrawler
    {
        public IEnumerable<string> GetOrganizations()
        {
            var root = new DirectoryInfo(Settings.Section.RepositoryPath);

            foreach (var directory in root.EnumerateDirectories())
            {
                try
                {
                    directory.EnumerateDirectories();
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                yield return directory.Name;
            }
        }

        public IEnumerable<OrganizationRepository> GetRepositoriesByOrganization(
            string organization)
        {
            var path = Path.Combine(Settings.Section.RepositoryPath, organization);
            var root = new DirectoryInfo(path);

            try
            {
                root.EnumerateDirectories();
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }

            var directories = new List<OrganizationRepository>();
            GetRepositories(directories, root, organization);

            return directories.OrderByDescending(x => x.Name);
        }

        private void GetRepositories(
            ICollection<OrganizationRepository> directories,
            DirectoryInfo directory,
            string relativePath)
        {
            if (directory.Name.EndsWith(".git"))
            {
                // Repository
                var path = RepositoryPath.Resolve(relativePath);

                directories.Add(
                    new OrganizationRepository
                        {
                            Name = path.RootPath
                        });
            }
            else
            {
                // Namespace
                foreach (var subDirectory in directory.EnumerateDirectories())
                {
                    var path = string.Concat(relativePath, "/", subDirectory.Name);
                    GetRepositories(directories, subDirectory, path);
                }
            }
        }
    }
}