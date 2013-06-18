using System.Web.Mvc;
using MarkdownSharp;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Mvc.Viewers
{
    public class MarkdownViewer : IContentViewer
    {
        public MvcHtmlString Render(RepositoryBlob blob)
        {
            var markdown = new Markdown();
            return new MvcHtmlString(markdown.Transform(blob.Content));
        }
    }
}