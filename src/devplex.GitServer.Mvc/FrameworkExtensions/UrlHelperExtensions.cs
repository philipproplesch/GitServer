using System.Text;
using System.Web.Mvc;
using devplex.GitServer.Core.Configuration;

namespace devplex.GitServer.Mvc.FrameworkExtensions
{
    public static class UrlHelperExtensions
    {
        public static string RepositoryCloneUrl(this UrlHelper instance, string path)
        {
            var request = instance.RequestContext.HttpContext.Request;

            var builder = new StringBuilder();
            builder.Append(Settings.UseSsl ? "https" : "http");
            builder.Append("://");
            builder.Append(request.Url.DnsSafeHost);

            if (request.ApplicationPath != "/")
            {
                builder.Append(request.ApplicationPath);
            }

            builder.Append("/");
            builder.Append(path);

            return builder.ToString();
        }
    }
}