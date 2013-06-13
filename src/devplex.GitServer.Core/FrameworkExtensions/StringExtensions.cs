using System.Security.Cryptography;
using System.Text;

namespace devplex.GitServer.Core.FrameworkExtensions
{
    public static class StringExtensions
    {
         public static string Hash(
             this string instance, 
             HashAlgorithm algorithm)
         {
             var bytes = Encoding.UTF8.GetBytes(instance);
             var hash = algorithm.ComputeHash(bytes);

             var builder = new StringBuilder();
             foreach (var b in hash)
             {
                 builder.Append(b.ToString("x2"));
             }

             return builder.ToString();
         }

        public static string MD5(
            this string instance)
        {
            return instance.Hash(new MD5CryptoServiceProvider());
        }
    }
}