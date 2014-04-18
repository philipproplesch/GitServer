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
                    .Include());

            bundles.Add(
                new ScriptBundle("~/scripts/modernizr")
                    .Include("~/Vendor/modernizr/modernizr.js"));

            bundles.Add(
                new ScriptBundle("~/scripts/app")
                    .Include(
                    "~/Vendor/jquery/dist/jquery.js",
                    "~/Vendor/fastclick/lib/fastclick.js",
                    "~/Vendor/foundation/js/foundation.js",
                    "~/Vendor/angular/angular.js",
                    "~/Scripts/devplex.GitServer.*"));
        }
    }
}