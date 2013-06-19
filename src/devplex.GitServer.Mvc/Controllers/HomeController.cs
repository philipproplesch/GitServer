using System.Linq;
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

            var model = new RepositoriesViewModel
                {
                    Organizations = crawler
                        .GetOrganizations()
                        .ToDictionary(
                            organisation => organisation,
                            crawler.GetRepositoriesByOrganization)
                };

            return View(model);
        }
    }
}
