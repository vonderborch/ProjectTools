namespace ProjectTools.Core.Templating.Generation
{
    /// <summary>
    /// What to do about Git when creating a new solution from a template
    /// </summary>
    public enum GitRepoMode
    {
        /// <summary>
        /// Create no repo
        /// </summary>
        NoRepo,

        /// <summary>
        /// Create a repo, but only initialize it
        /// </summary>
        NewRepoOnlyInit,

        /// <summary>
        /// Create a new repo
        /// </summary>
        NewRepoFull
    }
}
