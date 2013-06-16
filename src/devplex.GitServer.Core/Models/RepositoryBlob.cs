using System.Linq;

namespace devplex.GitServer.Core.Models
{
    public class RepositoryBlob
    {
        public string FileName { get; set; }
        public string Content { get; set; }

        public byte[] RawContent { get; set; }

        public string RawUrl { get; set; }

        public bool IsImage()
        {
            var extension = FileName.ToUpperInvariant();
            var extensions = new[] {".PNG", ".GIF", ".JPG", ".JPEG"};
            
            return extensions.Any(extension.EndsWith);
        }

        public bool IsMarkdown()
        {
            var extension = FileName.ToUpperInvariant();
            var extensions = new[] { ".MD", ".MARKDOWN" };

            return extensions.Any(extension.EndsWith);
        }

        public bool IsBinary()
        {
            // TODO: Detect binary file.
            var extension = FileName.ToUpperInvariant();
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
