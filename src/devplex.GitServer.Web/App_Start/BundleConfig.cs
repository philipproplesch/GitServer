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
                        "~/Content/normalize.css",
                        "~/Content/styles.css",
                        "~/Content/Prettify/prettify.css",
                        "~/Content/Prettify/Themes/sons-of-obsidian.css",
                        "~/Content/font-awesome.css"));

            bundles.Add(
                new ScriptBundle("~/scripts/app")
                    .Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.timeago.js",
                        "~/Scripts/Prettify/prettify.js",
                        "~/Scripts/devplex.gitserver*"));
        }
    }
}