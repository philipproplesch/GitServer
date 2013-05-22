using System.Web.Mvc;
using System.Web.Routing;

namespace devplex.GitServer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "InfoRefs",
                "{*path}",
                new { controller = "SmartHttp", action = "InfoRefs" },
                new { method = new HttpMethodConstraint("GET"), path = @".*\.git/info/refs" }
                );

            routes.MapRoute(
                "ReceivePack",
                "{*path}",
                new { controller = "SmartHttp", action = "ReceivePack" },
                new { method = new HttpMethodConstraint("POST"), path = @".*\.git/git-receive-pack" }
                );

            routes.MapRoute(
                "UploadPack",
                "{*path}",
                new { controller = "SmartHttp", action = "UploadPack" },
                new { method = new HttpMethodConstraint("POST"), path = @".*\.git/git-upload-pack" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}