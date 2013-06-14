using System;
using System.Web.Mvc;
using devplex.GitServer.Core.Git;
using devplex.GitServer.Mvc.ViewModels;

namespace devplex.GitServer.Mvc.Controllers
{
    public class RepositoryController : Controller
    {
        readonly Func<string, bool> _isReadMe = fileName =>
        {
            fileName = fileName.ToUpper();

            if (fileName.Contains("."))
            {
                var segments = fileName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                fileName = segments[0];
            }

            return fileName.StartsWith("README");
        };

        public ActionResult Index(string path)
        {
            var repositoryBrowser = new RepositoryBrowser();
            var repositoryPath = repositoryBrowser.SplitRepositoryPath(path);

            var model = new RepositoryViewModel();
            model.Path = repositoryPath;
            model.Branches = repositoryBrowser.GetBranches(repositoryPath.AbsoluteRootPath);
            model.ReadMe = repositoryBrowser.FindFile(repositoryPath, _isReadMe);

            return View(model);
        }

        public ActionResult Log(string branch, string path)
        {
            var repositoryBrowser = new RepositoryBrowser();
            var repositoryPath = repositoryBrowser.SplitRepositoryPath(path);

            var messages = repositoryBrowser.GetCommitMessages(branch, repositoryPath);

            return View(messages);
        }

        public ActionResult Tree(string branch, string path)
        {
            var repositoryBrowser = new RepositoryBrowser();
            var repositoryPath = repositoryBrowser.SplitRepositoryPath(path);

            var model = new TreeViewModel {
                Branch = branch,
                RepositoryPath = repositoryPath.RootPath,
                Tree = repositoryBrowser.GetRepositoryContent(branch, repositoryPath)
            };

            return View(model);
        }

        public ActionResult Blob(string branch, string path)
        {
            var repositoryBrowser = new RepositoryBrowser();
            var repositoryPath = repositoryBrowser.SplitRepositoryPath(path);

            var content = repositoryBrowser.GetBlobContent(branch, repositoryPath);
            if (content == null)
            {
                return HttpNotFound();
            }

            content.RawUrl = Url.Action("RawBlob", new { branch, path });

            return View(content);
        }

        public ActionResult RawBlob(string branch, string path)
        {
            var repositoryBrowser = new RepositoryBrowser();
            var repositoryPath = repositoryBrowser.SplitRepositoryPath(path);

            var content = repositoryBrowser.GetBlobContent(branch, repositoryPath);

            if (content == null)
            {
                return HttpNotFound();
            }

            return File(content.RawContent, "application/octet-stream", content.FileName);
        }

        public ActionResult ZipArchive(string branch, string path)
        {
            var repositoryBrowser = new RepositoryBrowser();
            var repositoryPath = repositoryBrowser.SplitRepositoryPath(path);

            var archive = repositoryBrowser.GenerateZipArchive(branch, repositoryPath);
            
            return File(archive.Data, "application/zip", archive.Name);
        }
    }
}