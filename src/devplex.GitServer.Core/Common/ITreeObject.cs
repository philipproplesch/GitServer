using System.Collections.Generic;

namespace devplex.GitServer.Core.Common
{
    public interface ITreeObject
    {
        int Order { get; }

        string Name { get; set; }
        string Path { get; set; }

        List<ITreeObject> Objects { get; set; }
    }
}
