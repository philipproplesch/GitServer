using System.Web.Mvc;

namespace devplex.GitServer.Mvc.FrameworkExtensions
{
    public static class UrlHelperExtensions
    {
         public static string CloneUrl(this UrlHelper instance, string path)
         {
             var baseUrl = instance.RequestContext.HttpContext.Request.Url.AbsoluteUri;
             if (baseUrl.EndsWith("/"))
             {
                 baseUrl = baseUrl.TrimEnd('/');
             }

             return string.Concat(baseUrl, path);
         }
    }
}