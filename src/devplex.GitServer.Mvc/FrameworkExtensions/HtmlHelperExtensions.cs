using System;
using System.Linq;
using System.Web.Mvc;
using MarkdownSharp;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.Extensions;
using devplex.GitServer.Core.FrameworkExtensions;
using devplex.GitServer.Core.Git;
using devplex.GitServer.Core.Models;
using devplex.GitServer.Mvc.Viewers;

namespace devplex.GitServer.Mvc.FrameworkExtensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RenderViewer(
             this HtmlHelper instance, RepositoryBlob blob)
        {
            Func<Type, IContentViewer> createInstance =
                type => (IContentViewer) Activator.CreateInstance(type);

            if (blob.IsBinary())
            {
                return new MvcHtmlString("// NO PREVIEW AVAILABLE");
            }

            var fileName = blob.FileName.ToUpper();

            var viewers = Settings.Section.Viewers;

            IContentViewer contentViewer = null;
            foreach (ViewerElement viewer in viewers)
            {
                foreach (ViewerExtensionElement element in viewer.Extensions)
                {
                    var extension = element.Extension.ToUpper();
                    if (fileName.EndsWith(extension))
                    {
                        contentViewer = createInstance(viewer.Type);
                        break;
                    }

                    if (contentViewer != null)
                    {
                        break;
                    }
                }
            }

            if (contentViewer == null)
            {
                contentViewer = createInstance(viewers.FallbackViewerType);
            }

            return contentViewer.Render(blob);
        }

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