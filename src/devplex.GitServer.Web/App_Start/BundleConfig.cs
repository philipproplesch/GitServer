using System.Web.Optimization;

namespace devplex.GitServer.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/content/css")
                    .Include(
                        "~/Content/app.css",
                        "~/Content/styles.css",
                        "~/Content/Prettify/prettify.css",
                        "~/Content/Prettify/Themes/sons-of-obsidian.css",
                        "~/Content/font-awesome.css"));

            bundles.Add(
                new ScriptBundle("~/scripts/modernizr")
                    .Include(
                        "~/Scripts/vendor/custom.modernizr.js"));

            bundles.Add(
                new ScriptBundle("~/scripts/app")
                    .Include(
                        "~/Scripts/vendor/jquery.js",
                        "~/Scripts/foundation/foundation.js",
                        "~/Scripts/foundation/foundation.*",
                        "~/Scripts/Prettify/prettify.js",
                        "~/Scripts/devplex.gitserver.js",
                        "~/Scripts/devplex.gitserver.*"));
        }
    }
}