using System.IO;
using GitSharp.Core.Transport;

namespace devplex.GitServer.Core.Git
{
    public class TransportPackage
    {
        public static void AdvertiseUploadPack(string path, Stream output)
        {
            var gitRepository = new GitRepository(path);

            var repository =
                Directory.Exists(gitRepository.AbsoluteRootPath)
                    ? gitRepository.Open()
                    : gitRepository.Init();

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
            var gitRepository = new GitRepository(path);

            using (var repository = gitRepository.Open())
            {
                var pack = new ReceivePack(repository);

                pack.SendAdvertisedRefs(
                    new RefAdvertiser.PacketLineOutRefAdvertiser(
                        new PacketLineOut(output)));
            }
        }

        public static void Upload(string path, Stream input, Stream output)
        {
            var gitRepository = new GitRepository(path);

            using (var repository = gitRepository.Open())
            {
                var pack = new UploadPack(repository);
                pack.setBiDirectionalPipe(false);
                pack.Upload(input, output, output);
            }
        }

        public static void Receive(string path, Stream input, Stream output)
        {
            var gitRepository = new GitRepository(path);

            using (var repository = gitRepository.Open())
            {
                var pack = new ReceivePack(repository);
                pack.setBiDirectionalPipe(false);
                pack.receive(input, output, output);
            }
        }
    }
}
