using System.Web.Optimization;

namespace devplex.GitServer.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/content/css")
                    .Include("~/Content/font-awesome.css"));
        }
    }
}