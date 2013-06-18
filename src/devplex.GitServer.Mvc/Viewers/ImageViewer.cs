using System.Web.Mvc;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.Viewers
{
    public class ImageViewer : IContentViewer
    {
        public MvcHtmlString Render(RepositoryBlob blob)
        {
            var builder = new TagBuilder("img");
            builder.Attributes.Add("src", blob.RawUrl);
            builder.Attributes.Add("alt", blob.FileName);

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}