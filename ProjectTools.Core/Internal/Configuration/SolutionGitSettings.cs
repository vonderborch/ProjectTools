namespace ProjectTools.Core.Internal.Configuration
{
    public class SolutionGitSettings
    {
        /// <summary>
        /// The repo name
        /// </summary>
        public string RepoName;

        /// <summary>
        /// The repo owner
        /// </summary>
        public string RepoOwner;

        /// <summary>
        /// The repo mode
        /// </summary>
        public GitRepoMode RepoMode = GitRepoMode.NoRepo;

        /// <summary>
        /// The is private
        /// </summary>
        public bool IsPrivate = true;
    }
}
