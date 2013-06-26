using System;
using System.Linq;
using System.Web.Mvc;
using MarkdownSharp;
using devplex.GitServer.Core.Common;
using devplex.GitServer.Core.Configuration;
using devplex.GitServer.Core.FrameworkExtensions;
using devplex.GitServer.Core.Models;
using devplex.GitServer.Core.Versioning;

namespace devplex.GitServer.Mvc.FrameworkExtensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RenderViewer(
             this HtmlHelper instance, RepositoryBlob blob)
        {
            Func<Type, IContentViewer> createInstance =
                type => (IContentViewer) Activator.CreateInstance(type);

            Func<string, ExtensionCollection, bool> matchesExtension =
                (fileName, extensions) => {
                    fileName = fileName.ToUpper();

                    return extensions
                        .Cast<ExtensionElement>()
                        .Select(element => element.Extension.ToUpper())
                        .Any(extension => fileName.EndsWith(extension));
                };


            var section = Settings.Section;
            if (matchesExtension(blob.FileName, section.Extensions))
            {
                return new MvcHtmlString("// NO PREVIEW AVAILABLE");
            }

            var viewers = section.Viewers;

            var contentViewer =
                viewers
                    .Cast<ViewerElement>()
                    .Where(viewer => matchesExtension(blob.FileName, viewer.Extensions))
                    .Select(viewer => createInstance(viewer.Type))
                    .FirstOrDefault() ??
                createInstance(viewers.FallbackViewerType);

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
            var type = typeof (GitVersioningSystem);
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