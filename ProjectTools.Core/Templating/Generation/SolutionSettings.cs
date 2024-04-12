using System.Text.Json.Serialization;
using ProjectTools.Core.Implementations.DotSln;
using ProjectTools.Core.PropertyHelpers;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Templating.Generation;

/// <summary>
/// Settings for the solution being generated
/// </summary>
[JsonDerivedType(typeof(SolutionSettings), "base")]
[JsonDerivedType(typeof(DotSlnSolutionSettings), "dotsln")]
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
    /// The field special text for the GitRepoName field
    /// </summary>
    public const string FieldSpecialTextGitRepoPrivate = "GIT_REPO_PRIVATE";

    /// <summary>
    /// The author of the solution
    /// </summary>
    [SolutionSettingFieldMetadata("Author", nameof(TemplateSettings.DefaultAuthor), PropertyType.String, 2)]
    public required string Author;

    /// <summary>
    /// The description of the solution
    /// </summary>
    [SolutionSettingFieldMetadata("Description", nameof(TemplateSettings.DefaultDescription), PropertyType.String, 20)]
    public required string Description;

    /// <summary>
    /// The git repo mode
    /// </summary>
    [SolutionSettingFieldMetadata("Git Repo Mode", FieldSpecialTextGitRepoMode, PropertyType.Enum,
        defaultValue: GitRepoMode.NoRepo, order: 100)]
    public GitRepoMode GitRepoMode = GitRepoMode.NoRepo;

    /// <summary>
    /// The git repo name
    /// </summary>
    [SolutionSettingFieldMetadata("Git Repo Name", FieldSpecialTextGitRepoName, PropertyType.String, defaultValue: "",
        order: 101, requiredFieldName: "GitRepoMode", disallowedFieldValue: GitRepoMode.NoRepo)]
    public string GitRepoName = string.Empty;

    /// <summary>
    /// The git repo owner
    /// </summary>
    [SolutionSettingFieldMetadata("Git Repo Owner", FieldSpecialTextGitRepoOwner, PropertyType.String, defaultValue: "",
        order: 102, requiredFieldName: "GitRepoMode", disallowedFieldValue: GitRepoMode.NoRepo)]
    public string GitRepoOwner = string.Empty;

    /// <summary>
    /// The git repo owner
    /// </summary>
    [SolutionSettingFieldMetadata("Private Git Repo", FieldSpecialTextGitRepoOwner, PropertyType.Bool,
        defaultValue: false, order: 103, requiredFieldName: "GitRepoMode", disallowedFieldValue: GitRepoMode.NoRepo)]
    public bool GitRepoPrivate = false;

    /// <summary>
    /// The nuget license expression
    /// </summary>
    [SolutionSettingFieldMetadata("License Expression", nameof(TemplateSettings.LicenseExpression), PropertyType.String,
        10)]
    [AllowedValue(Constants.LICENSE_EXPRESSIONS, PropertyType.StringListComma)]
    public required string LicenseExpression;

    /// <summary>
    /// The name of the solution
    /// </summary>
    [SolutionSettingFieldMetadata("Solution Name", nameof(TemplateSettings.DefaultSolutionName), PropertyType.String,
        0)]
    public required string Name;

    /// <summary>
    /// The starting version of the solution
    /// </summary>
    [SolutionSettingFieldMetadata("Starting Version", nameof(TemplateSettings.DefaultStartingVersion),
        PropertyType.String, 1)]
    public required string Version;
}