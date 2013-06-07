using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
    public interface ITreeDirectory
    {
        string Name { get; set; }
        string Path { get; set; }
        List<ITreeDirectory> Directories { get; set; }
    }
}
