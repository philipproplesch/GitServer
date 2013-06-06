using System.Web.Mvc;
using devplex.GitServer.Core.IO;
using devplex.GitServer.Mvc.ViewModels;

namespace devplex.GitServer.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var crawler = new DirectoryCrawler();
            var absolutePath = crawler.GetAbsolutePath(string.Empty);

            var tree = crawler.GetTree(absolutePath);

            var model = new DirectoryTreeViewModel
                {
                    Tree = tree
                };

            return View(model);
        }

        public ActionResult Help()
        {
          return View();
        }
    }
}
