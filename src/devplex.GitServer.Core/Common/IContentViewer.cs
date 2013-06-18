using System.Web.Mvc;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Common
{
    public interface IContentViewer
    {
        MvcHtmlString Render(RepositoryBlob blob);
    }
}