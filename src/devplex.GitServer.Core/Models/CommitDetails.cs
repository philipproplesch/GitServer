using System.Collections.Generic;

namespace devplex.GitServer.Core.Models
{
	public class CommitDetails
	{
		public CommitMessage Message { get; set; }
		public IEnumerable<FileDiff> FileDiffs { get; set; }
	}
}
