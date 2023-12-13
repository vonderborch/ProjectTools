using System.Text.Json.Serialization;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Implementations.DotSln;
using ProjectTools.Core.Templating.Common;

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
        /// The field special text for the CreateGitRepo field
        /// </summary>
        public const string FieldSpecialTextCreateGitRepo = "CREATE_GIT_REPO";

        /// <summary>
        /// The field special text for the GitRepoMode field
        /// </summary>
        public const string FieldSpecialTextGitRepoMode = "GIT_REPO_MODE";

        /// <summary>
        /// The field special text for the GitRepoName field
        /// </summary>
        public const string FieldSpecialTextGitRepoName = "GIT_REPO_NAME";

        /// <summary>
        /// The field special text for the GitRepoOwner field
        /// </summary>
        public const string FieldSpecialTextGitRepoOwner = "GIT_REPO_OWNER";

        /// <summary>
        /// The author of the solution
        /// </summary>
        [SolutionSettingFieldMetadata("Author", nameof(TemplateSettings.DefaultAuthor), PropertyType.String, order: 2)]
        public required string Author;

        /// <summary>
        /// The description of the solution
        /// </summary>
        [SolutionSettingFieldMetadata("Description", nameof(TemplateSettings.DefaultDescription), PropertyType.String, order: 10)]
        public required string Description;

        /// <summary>
        /// The git repo mode
        /// </summary>
        [SolutionSettingFieldMetadata("Git Repo Mode", FieldSpecialTextGitRepoMode, PropertyType.Enum, order: 100)]
        public GitRepoMode GitRepoMode = GitRepoMode.NoRepo;

        /// <summary>
        /// The git repo name
        /// </summary>
        [SolutionSettingFieldMetadata("Git Repo Name", FieldSpecialTextGitRepoName, PropertyType.String, order: 101, requiredFieldName: "GitRepoMode", requiredFieldValue: GitRepoMode.NoRepo)]
        public string GitRepoName = string.Empty;

        /// <summary>
        /// The git repo owner
        /// </summary>
        [SolutionSettingFieldMetadata("Git Repo Owner", FieldSpecialTextGitRepoOwner, PropertyType.String, order: 102, requiredFieldName: "GitRepoMode", requiredFieldValue: GitRepoMode.NoRepo)]
        public string GitRepoOwner = string.Empty;

        /// <summary>
        /// The name of the solution
        /// </summary>
        [SolutionSettingFieldMetadata("Solution Name", nameof(TemplateSettings.DefaultSolutionName), PropertyType.String, order: 0)]
        public required string Name;

        /// <summary>
        /// The starting version of the solution
        /// </summary>
        [SolutionSettingFieldMetadata("Starting Version", nameof(TemplateSettings.DefaultStartingVersion), PropertyType.String, order: 1)]
        public required string Version;
    }
}
