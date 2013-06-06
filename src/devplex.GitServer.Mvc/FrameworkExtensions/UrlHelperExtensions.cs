using System.Configuration;
using System.Web.Mvc;

namespace devplex.GitServer.Mvc.FrameworkExtensions
{
  public static class UrlHelperExtensions
  {
    public static string RepositoryCloneUrl(this UrlHelper instance, string path)
    {

      var baseUrl = instance.RequestContext.HttpContext.Request.Url.AbsoluteUri;
      if (baseUrl.EndsWith("/"))
      {
        baseUrl = baseUrl.TrimEnd('/');
      }

      if ("TRUE".Equals(
        ConfigurationManager.AppSettings["GitServer.UseSSL"],
        System.StringComparison.OrdinalIgnoreCase) &&
        !baseUrl.Contains("https://"))
      {
        baseUrl = baseUrl.Replace("http://", "https://");
      }

      return string.Concat(baseUrl, path);
    }
  }
}