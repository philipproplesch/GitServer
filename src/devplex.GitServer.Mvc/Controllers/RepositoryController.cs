using System.Web.Mvc;
using devplex.GitServer.Core.Git;

namespace devplex.GitServer.Mvc.Controllers
{
    public class RepositoryController : Controller
    {
        public ActionResult Log(string branch, string path)
        {
            var repositoryBrowser = new RepositoryBrowser();

            var messages = 
                repositoryBrowser.GetCommitMessages(branch, path);

            return View(messages);
        }
    }
}