using System.Linq;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Extensions
{
    public static class RepositoryBlobExtensions
    {
        public static bool IsImage(this RepositoryBlob instance)
        {
            var extension = instance.FileName.ToUpperInvariant();
            var extensions = new[] {
                ".PNG", ".GIF", ".JPG", ".JPEG"
            };

            return extensions.Any(extension.EndsWith);
        }

        public static bool IsMarkdown(this RepositoryBlob instance)
        {
            var extension = instance.FileName.ToUpperInvariant();
            var extensions = new[] {
                ".MD", ".MARKDOWN"
            };

            return extensions.Any(extension.EndsWith);
        }

        public static bool IsBinary(this RepositoryBlob instance)
        {
            // TODO: Detect binary file.

            var extension = instance.FileName.ToUpperInvariant();
            var extensions = new[] {
                ".EXE",
                ".PPTX", ".PPT",
                ".DOC", ".DOCX",
                ".XLS", ".XLSX",
                ".SNK",
                ".ICO"
            };

            return extensions.Any(extension.EndsWith);
        } 
    }
}