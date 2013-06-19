using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using devplex.GitServer.Core.IO;
using devplex.GitServer.Core.Models;
using devplex.GitServer.Mvc.ViewModels;

namespace devplex.GitServer.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var crawler = new DirectoryCrawler();

            var model = new RepositoriesViewModel();
            
            var data = new Dictionary<string, IEnumerable<OrganizationRepository>>();
            foreach (var organization in crawler.GetOrganizations())
            {
                var repositories =
                    crawler
                        .GetRepositoriesByOrganization(organization)
                        .Where(x => x.HasBranches)
                        .ToList();

                if (repositories.Any())
                {
                    data.Add(organization, repositories);
                }
            }

            model.Organizations = data;

            return View(model);
        }
    }
}
