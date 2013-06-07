using System.IO;
using System.Linq;
using GitSharp;
using devplex.GitServer.Core.Configuration;

namespace devplex.GitServer.Core.Git
{
    public class RepositoryBrowser
    {
        public static string GetRepositoryPath(string path)
        {
            var root = Settings.GetValue("GitServer.GitRoot");
            return Path.Combine(root, path + ".git");
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
    }
}
