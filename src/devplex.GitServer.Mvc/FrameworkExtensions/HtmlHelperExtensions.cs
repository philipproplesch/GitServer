using System.Web.Mvc;
using MarkdownSharp;
using devplex.GitServer.Core.FrameworkExtensions;
using devplex.GitServer.Core.Git;

namespace devplex.GitServer.Mvc.FrameworkExtensions
{
    public static class HtmlHelperExtensions
    {
         public static MvcHtmlString RenderMarkdown(
             this HtmlHelper instance, string input)
         {
             var markdown = new Markdown();
             return new MvcHtmlString(markdown.Transform(input));
         }

         public static MvcHtmlString Gravatar(
            this HtmlHelper instance, string mailAddress)
        {
            var hash = mailAddress.MD5();

            var url = "http://www.gravatar.com/";
            if (instance.ViewContext.HttpContext.Request.IsSecureConnection)
            {
                url = "https://secure.gravatar.com/";
            }

            url += string.Concat("avatar/", hash, "?s=50");

            var tag = new TagBuilder("img");
            tag.Attributes.Add("src", url);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static string Version(
            this HtmlHelper instance)
        {
            var type = typeof (GitRepository);
            var version = type.Assembly.GetName().Version;

            return string.Format(
                "{0}.{1}.{2}.{3}",
                version.Major,
                version.Minor,
                version.Build,
                version.Revision);
        }
    }
}