using System.Web.Mvc;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.Viewers
{
    public class ImageViewer : IContentViewer
    {
        public MvcHtmlString Render(RepositoryBlob blob)
        {
            var tag = new TagBuilder("img");
            tag.Attributes.Add("src", blob.RawUrl);
            tag.Attributes.Add("alt", blob.FileName);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }
    }
}