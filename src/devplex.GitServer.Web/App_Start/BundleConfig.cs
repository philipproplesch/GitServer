using System.Web.Optimization;

namespace devplex.GitServer.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Ignore("*.coffee");

            bundles.Add(
                new StyleBundle("~/content/css")
                    .Include("~/Content/Styles/main.css"));

            bundles.Add(
                new ScriptBundle("~/scripts/modernizr")
                    .Include("~/bower_components/modernizr/modernizr.js"));

            bundles.Add(
                new ScriptBundle("~/scripts/app")
                    .Include(
                    "~/bower_components/jquery/dist/jquery.js",
                    "~/bower_components/fastclick/lib/fastclick.js",
                    "~/bower_components/foundation/js/foundation.js",
                    "~/bower_components/angular/angular.js",
                    "~/Scripts/devplex.GitServer.js"));
        }
    }
}