using LibGit2Sharp;
using devplex.GitServer.Core.Models;

namespace devplex.GitServer.Core.Extensions
{
	public static class CommitExtensions
	{
		public static CommitMessage ToCommitMessage(this Commit instance)
		{
			return new CommitMessage
				{
					Hash = instance.Sha,
					ShortHash = instance.Sha,
					Message = instance.Message,
					ShortMessage = instance.MessageShort,
					AuthorName = instance.Author.Name,
					AuthorMailAddress = instance.Author.Email,
					Timestamp = instance.Author.When.UtcDateTime
				};
		}
	}
}