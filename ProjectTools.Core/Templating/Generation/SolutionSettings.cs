using System.Text.Json.Serialization;
using ProjectTools.Core.Implementations.DotSln;

namespace ProjectTools.Core.Templating.Generation
{
    /// <summary>
    /// Settings for the solution being generated
    /// </summary>
    [JsonDerivedType(typeof(SolutionSettings), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(DotSlnSolutionSettings), typeDiscriminator: "dotsln")]
    public class SolutionSettings
    {
        /// <summary>
        /// The author of the solution
        /// </summary>
        public required string Author;

        /// <summary>
        /// The company of the solution
        /// </summary>
        public required string Company;

        /// <summary>
        /// The description of the solution
        /// </summary>
        public required string Description;

        /// <summary>
        /// Whether the Git Repo is private or not
        /// </summary>
        public bool GitRepoIsPrivate = true;

        /// <summary>
        /// The git repo mode
        /// </summary>
        public GitRepoMode GitRepoMode = GitRepoMode.NoRepo;

        /// <summary>
        /// The git repo name
        /// </summary>
        public string? GitRepoName;

        /// <summary>
        /// The git repo owner
        /// </summary>
        public string? GitRepoOwner;

        /// <summary>
        /// The name of the solution
        /// </summary>
        public required string Name;

        /// <summary>
        /// The starting version of the solution
        /// </summary>
        public required string Version;
    }
}
