using System;
using System.Linq;
using GitSharp;

namespace devplex.GitServer.Core.Extensions
{
    public static class TreeExtensions
    {
         public static Tree FindSubPath(this Tree instance, string subPath)
         {
             var segments =
                        subPath.Split(
                            new[] { "/" },
                            StringSplitOptions.RemoveEmptyEntries);

             var newRoot = instance;
             foreach (var segment in segments)
             {
                 var tree =
                     newRoot.Trees.FirstOrDefault(
                         child =>
                         child.Name.Equals(
                             segment,
                             StringComparison.Ordinal));

                 if (tree != null)
                 {
                     newRoot = tree;
                 }
                 else
                 {
                     return null;
                 }
             }

             return newRoot;
         }

        public static Leaf FindFile(this Tree instance, string subPath)
        {
            var segments =
                subPath.Split(
                    new[] {"/"},
                    StringSplitOptions.RemoveEmptyEntries)
                       .ToList();

            // Extract the file name from the path.
            var fileName = segments[segments.Count - 1];

            // Remove file name from path.
            segments.RemoveAt(segments.Count - 1);

            var newSubPath = string.Join("/", segments);

            var newRoot = instance.FindSubPath(newSubPath);
            if (newRoot == null)
            {
                return null;
            }

            // Finde the requested file in the current tree.
            return
                newRoot.Leaves.FirstOrDefault(
                    child =>
                    child.Name.Equals(
                        fileName,
                        StringComparison.OrdinalIgnoreCase));
        }
    }
}