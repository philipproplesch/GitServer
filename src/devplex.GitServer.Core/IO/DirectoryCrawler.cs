using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.Git;
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

            return directories.OrderBy(x => x.Name);
        }

        private void GetRepositories(
            ICollection<OrganizationRepository> directories,
            DirectoryInfo directory,
            string relativePath)
        {
            if (directory.Name.EndsWith(".git"))
            {
                // Repository
                var repository = new GitRepository(relativePath);

                directories.Add(
                    new OrganizationRepository
                        {
                            Name = repository.RootPath,
                            HasBranches = repository.GetBranches().Any()
                        });
            }
            else
            {
                // Namespace
                foreach (var subDirectory in directory.EnumerateDirectories())
                {
                    try
                    {
                        subDirectory.EnumerateDirectories();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }

                    var path = string.Concat(relativePath, "/", subDirectory.Name);
                    GetRepositories(directories, subDirectory, path);
                }
            }
        }
    }
}