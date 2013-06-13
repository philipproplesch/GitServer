using System.IO;
using System.Web;
using ICSharpCode.SharpZipLib.GZip;

namespace devplex.GitServer.Core.FrameworkExtensions
{
    public static class HttpRequestBaseExtensions
    {
        public static Stream GetRequestStream(this HttpRequestBase instance)
        {
            var contentEncoding = instance.Headers["Content-Encoding"];

            if (!string.IsNullOrEmpty(contentEncoding) && 
                contentEncoding.Equals("gzip"))
            {
                return new GZipInputStream(instance.InputStream);
            }

            return instance.InputStream;
        }
    }
}
