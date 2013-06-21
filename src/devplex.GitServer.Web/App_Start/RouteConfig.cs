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
                name: "Repository",
                url: "repository/{*path}",
                defaults: new { controller = "Repository", action = "Index" }
            );

            routes.MapRoute(
                name: "RepositoryTree",
                url: "tree/{branch}/{*path}",
                defaults: new { controller = "Repository", action = "Tree" }
            );

            routes.MapRoute(
                name: "RepositoryBlob",
                url: "blob/{branch}/{*path}",
                defaults: new { controller = "Repository", action = "Blob" }
            );

            routes.MapRoute(
                name: "RepositoryRawBlob",
                url: "raw/{branch}/{*path}",
                defaults: new { controller = "Repository", action = "RawBlob" }
            );

            routes.MapRoute(
                name: "RepositoryZipArchive",
                url: "zip/{branch}/{*path}",
                defaults: new { controller = "Repository", action = "ZipArchive" }
            );

            routes.MapRoute(
                name: "RepositoryLog",
                url: "log/{branch}/{*path}",
                defaults: new { controller = "Repository", action = "Log" }
            );

            routes.MapRoute(
                name: "RepositoryCommits",
                url: "commit/{hash}/{*path}",
                defaults: new { controller = "Repository", action = "Commit" }
            );

            // API ;)
            routes.MapRoute(
                name: "RepositoriesByOrganization",
                url: "api/repositories/{organization}",
                defaults: new { controller = "Home", action = "Repositories", organization = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}