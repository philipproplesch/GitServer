using System.Web;

namespace devplex.GitServer.Core.FrameworkExtensions
{
    public static class HttpResponseBaseExtensions
    {
        public static void WriteNoCache(this HttpResponseBase instance)
        {
            instance.AddHeader(
                "Cache-Control", "no-cache, no-store, must-revalidate");

            instance.AddHeader(
                "Pragma", "no-cache");

            instance.AddHeader(
                "Expires", "0");
        }

        public static void PacketWrite(this HttpResponseBase instance, string content)
        {
            var result =
                string.Concat(
                    (content.Length + 4).ToString("x").PadLeft(4, '0'),
                    content);

            instance.Write(result);
        }

        public static void PacketFlush(this HttpResponseBase instance)
        {
            instance.Write("0000");
        }
    }
}