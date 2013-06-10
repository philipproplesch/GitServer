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
                        "~/Content/font-awesome.css"));
        }
    }
}