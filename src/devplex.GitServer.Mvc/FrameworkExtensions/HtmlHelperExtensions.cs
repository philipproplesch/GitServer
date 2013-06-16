using System.Web.Mvc;
using MarkdownSharp;
using devplex.GitServer.Core.FrameworkExtensions;

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
    }
}