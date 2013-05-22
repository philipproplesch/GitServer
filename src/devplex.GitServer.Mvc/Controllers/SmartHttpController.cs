using System;
using System.Web.Mvc;
using devplex.GitServer.Core;
using devplex.GitServer.Core.FrameworkExtensions;
using devplex.GitServer.Mvc.ModelBinders;

namespace devplex.GitServer.Mvc.Controllers
{
    public class SmartHttpController : Controller
    {
        public ActionResult InfoRefs(
            [ModelBinder(typeof(SmartHttpPathBinder))] string path,
            string service)
        {
            Response.ContentType =
                string.Format(
                    "application/x-{0}-advertisement",
                    service);

            Response.Charset = string.Empty;
            Response.WriteNoCache();

            Response.PacketWrite(string.Format("# service={0}\n", service));
            Response.PacketFlush();

            if (service.Equals("git-upload-pack", StringComparison.OrdinalIgnoreCase))
            {
                SmartHttpPackage.AdvertiseUploadPack(
                    path, Response.OutputStream);
            }
            else if (service.Equals("git-receive-pack", StringComparison.OrdinalIgnoreCase))
            {
                SmartHttpPackage.AdvertiseReceivePack(
                    path, Response.OutputStream);
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ReceivePack(
            [ModelBinder(typeof(SmartHttpPathBinder))] string path)
        {
            Response.ContentType = "application/x-git-receive-pack-result";
            Response.Charset = string.Empty;
            Response.WriteNoCache();

            SmartHttpPackage.Receive(
                path, Request.GetRequestStream(), Response.OutputStream);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UploadPack(
            [ModelBinder(typeof(SmartHttpPathBinder))] string path)
        {
            Response.ContentType = "application/x-git-upload-pack-result";
            Response.Charset = string.Empty;
            Response.WriteNoCache();

            SmartHttpPackage.Upload(
                path, Request.GetRequestStream(), Response.OutputStream);

            return new EmptyResult();
        }
    }
}
