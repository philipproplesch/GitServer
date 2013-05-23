using System.IO;
using GitSharp;
using GitSharp.Core.Transport;
using devplex.GitServer.Core.Configuration;

namespace devplex.GitServer.Core.Git
{
    public class SmartHttpPackage
    {
        private static string GetRepositoryPath(string path)
        {
            var root = Settings.GetValue("GitServer.GitRoot");
            return Path.Combine(root, path + ".git");
        }

        public static void AdvertiseUploadPack(string path, Stream output)
        {
            var absolutePath = GetRepositoryPath(path);

            // TODO: Replace with GUI and throw error/return 404.
            var repository =
                !Directory.Exists(absolutePath)
                    ? Repository.Init(absolutePath, true)
                    : new Repository(absolutePath);

            using (repository)
            {
                var pack = new UploadPack(repository);

                pack.sendAdvertisedRefs(
                    new RefAdvertiser.PacketLineOutRefAdvertiser(
                        new PacketLineOut(output)));
            }
        }

        public static void AdvertiseReceivePack(string path, Stream output)
        {
            var absolutePath = GetRepositoryPath(path);

            using (var repository = new Repository(absolutePath))
            {
                var pack = new ReceivePack(repository);

                pack.SendAdvertisedRefs(
                    new RefAdvertiser.PacketLineOutRefAdvertiser(
                        new PacketLineOut(output)));
            }
        }

        public static void Upload(string path, Stream input, Stream output)
        {
            var absolutePath = GetRepositoryPath(path);

            using (var repository = new Repository(absolutePath))
            {
                var pack = new UploadPack(repository);
                pack.setBiDirectionalPipe(false);
                pack.Upload(input, output, output);
            }
        }

        public static void Receive(string path, Stream input, Stream output)
        {
            var absolutePath = GetRepositoryPath(path);

            using (var repository = new Repository(absolutePath))
            {
                var pack = new ReceivePack(repository);
                pack.setBiDirectionalPipe(false);
                pack.receive(input, output, output);
            }
        } 
    }
}
