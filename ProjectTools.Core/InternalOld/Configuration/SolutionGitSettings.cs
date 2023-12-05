namespace ProjectTools.Core.Internal.Configuration
{
    /// <summary>
    /// Git settings for the solution being generated
    /// </summary>
    public class SolutionGitSettings
    {
        /// <summary>
        /// The is private
        /// </summary>
        public bool IsPrivate = true;

        /// <summary>
        /// The repo mode
        /// </summary>
        public GitRepoMode RepoMode = GitRepoMode.NoRepo;

        /// <summary>
        /// The repo name
        /// </summary>
        public string RepoName;

        /// <summary>
        /// The repo owner
        /// </summary>
        public string RepoOwner;
    }
}
