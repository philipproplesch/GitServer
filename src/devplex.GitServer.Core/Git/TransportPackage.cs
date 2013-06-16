using System.IO;
using GitSharp;
using GitSharp.Core.Transport;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Git
{
    public class TransportPackage
    {
        public static void AdvertiseUploadPack(string path, Stream output)
        {
            var repositoryPath = RepositoryPath.Resolve(path);

            var repository =
                !Directory.Exists(repositoryPath.AbsoluteRootPath)
                    ? Repository.Init(repositoryPath.AbsoluteRootPath, true)
                    : new Repository(repositoryPath.AbsoluteRootPath);

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
            var repositoryPath = RepositoryPath.Resolve(path);

            using (var repository = new Repository(repositoryPath.AbsoluteRootPath))
            {
                var pack = new ReceivePack(repository);

                pack.SendAdvertisedRefs(
                    new RefAdvertiser.PacketLineOutRefAdvertiser(
                        new PacketLineOut(output)));
            }
        }

        public static void Upload(string path, Stream input, Stream output)
        {
            var repositoryPath = RepositoryPath.Resolve(path);

            using (var repository = new Repository(repositoryPath.AbsoluteRootPath))
            {
                var pack = new UploadPack(repository);
                pack.setBiDirectionalPipe(false);
                pack.Upload(input, output, output);
            }
        }

        public static void Receive(string path, Stream input, Stream output)
        {
            var repositoryPath = RepositoryPath.Resolve(path);

            using (var repository = new Repository(repositoryPath.AbsoluteRootPath))
            {
                var pack = new ReceivePack(repository);
                pack.setBiDirectionalPipe(false);
                pack.receive(input, output, output);
            }
        }
    }
}
