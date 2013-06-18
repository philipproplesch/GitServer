using System.Web;
using System.Web.Mvc;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.Viewers
{
    public class RawViewer : IContentViewer
    {
        public MvcHtmlString Render(RepositoryBlob blob)
        {
            var tag = new TagBuilder("pre");
            tag.AddCssClass("prettyprint linenums");

            tag.InnerHtml = HttpUtility.HtmlEncode(blob.Content);

            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }
    }
}