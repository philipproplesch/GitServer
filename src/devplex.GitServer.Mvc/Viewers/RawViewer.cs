using System.Web.Mvc;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.Viewers
{
    public class RawViewer : IContentViewer
    {
        public MvcHtmlString Render(RepositoryBlob blob)
        {
            var builder = new TagBuilder("pre");
            builder.AddCssClass("prettyprint linenums");

            builder.InnerHtml = blob.Content;

            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }
    }
}