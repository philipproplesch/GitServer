using System;
using System.Web.Mvc;
using devplex.GitServer.Core.Versioning;
using devplex.GitServer.Mvc.ViewModels;

namespace devplex.GitServer.Mvc.Controllers
{
    public class RepositoryController : Controller
    {
        private readonly Func<string, bool>
            _isReadMe = fileName =>
                        fileName.ToUpperInvariant().StartsWith("README");

        public ActionResult Index(
            string path)
        {
            var repository = new GitVersioningSystem(path);

            var model = new RepositoryViewModel {
                RepositoryPath = repository.RootPath,
                Branches = repository.GetBranches(),
                ReadMe = repository.FindAndReadFile(_isReadMe)
            };

            return View(model);
        }

        public ActionResult Log(
            string branch, string path, int skip = 0, int take = 50)
        {
            var repository = new GitVersioningSystem(path, branch);

            var model = new CommitHistoryViewModel {
                Branch = branch,
                RepositoryPath = repository.RootPath,
                Commits = repository.GetCommitMessages(skip, take)
            };

            return View(model);
        }

        public ActionResult Commit(
            string branch, string hash, string path)
        {
            var repository = new GitVersioningSystem(path,branch);
            var details = repository.GetCommitDetails(hash);

            var model = new CommitDetailsViewModel {
                Branch = branch,
                RepositoryPath = repository.RootPath,
                Files = details.FileDiffs,
                CommitMessage = details.Message
            };

            return View(model);
        }

        public ActionResult Tree(
            string branch, string path, bool commits = false)
        {
            var repository = new GitVersioningSystem(path, branch);

            string parentSubPath = null;

            if (!string.IsNullOrEmpty(repository.SubPath))
            {
                parentSubPath = repository.SubPath;

                var index = repository.SubPath.LastIndexOf('/');
                if (index == -1)
                {
                    parentSubPath = string.Empty;
                }
                else if (index > -1)
                {
                    parentSubPath = repository.SubPath.Substring(0, index);
                }
            }

            var model = new RepositoryTreeViewModel {
                Branch = branch,
                RepositoryPath = repository.RootPath,
                ParentSubPath = parentSubPath,
                Tree = repository.GetRepositoryContent(commits)
            };

            var pjax = Request.Headers["X-PJAX"] != null;
            if (pjax || commits)
            {
                return PartialView("_RepositoryTree", model);
            }

            return View(model);
        }

        public ActionResult Blob(
            string branch, string path)
        {
            var repository = new GitVersioningSystem(path, branch);

            var content = repository.GetBlobContent();
            if (content == null)
            {
                return HttpNotFound();
            }

            content.RawUrl = Url.Action("RawBlob", new { branch, path });

            return View(content);
        }

        public ActionResult RawBlob(
            string branch, string path)
        {
            var repository = new GitVersioningSystem(path, branch);

            var content = repository.GetBlobContent();
            if (content == null)
            {
                return HttpNotFound();
            }

            return File(content.RawContent, "application/octet-stream", content.FileName);
        }

        public ActionResult ZipArchive(
            string branch, string path)
        {
            var repository = new GitVersioningSystem(path, branch);
            var archive = repository.GenerateZipArchive();
            
            return File(archive.Data, "application/zip", archive.Name);
        }
    }
}