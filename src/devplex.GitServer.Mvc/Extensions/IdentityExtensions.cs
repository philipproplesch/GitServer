using System;
using System.Security.Principal;

namespace devplex.GitServer.Mvc.Extensions
{
  public static class IdentityExtensions
  {
    public static string GetUserName(this IIdentity identity)
    {
      var name = identity.Name;
      var backSlashIndex = name.IndexOf("\\", StringComparison.Ordinal);
      if (backSlashIndex > -1)
      {
        name = name.Substring(backSlashIndex + 1);
      }
      return name;
    }
  }
}
