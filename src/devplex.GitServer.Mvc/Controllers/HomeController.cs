using System.Web.Mvc;
using devplex.GitServer.Core.IO;

namespace devplex.GitServer.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var crawler = new DirectoryCrawler();
            return View(crawler.GetOrganizations());
        }

        public JsonResult Repositories(string organization)
        {
            var crawler = new DirectoryCrawler();

            var repositories =
                crawler.GetRepositoriesByOrganization(organization);

            foreach (var repository in repositories)
            {
                repository.Path = Url.Action("Index", "Repository", new { path = repository.Name });
            }

            return Json(
                repositories,
                JsonRequestBehavior.AllowGet);
        }
    }
}
