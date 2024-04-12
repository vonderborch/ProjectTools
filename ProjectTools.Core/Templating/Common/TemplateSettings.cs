using System.Diagnostics;
using System.Text.Json.Serialization;
using ProjectTools.Core.Implementations.DotSln;
using ProjectTools.Core.PropertyHelpers;

namespace ProjectTools.Core.Templating.Common;

/// <summary>
/// Information on template settings
/// </summary>
[DebuggerDisplay("{DefaultSolutionName}")]
[JsonDerivedType(typeof(TemplateSettings), "base")]
[JsonDerivedType(typeof(DotSlnTemplateSettings), "dotsln")]
public class TemplateSettings
{
    /// <summary>
    /// The files and directories to remove from a new solution using this template
    /// </summary>
    [TemplateFieldMetadata("Cleanup Files And Directories (comma-separated)", PropertyType.StringListComma, 301)]
    public List<string> CleanupFilesAndDirectories = [];

    /// <summary>
    /// The default author for a new solution using this template
    /// </summary>
    [TemplateFieldMetadata("Default Author", PropertyType.String, 2)]
    public string DefaultAuthor = string.Empty;

    /// <summary>
    /// The default description for a new solution using this template
    /// </summary>
    [TemplateFieldMetadata("Default Description", PropertyType.String, 10)]
    public string DefaultDescription = string.Empty;

    /// <summary>
    /// The default solution name for a new solution using this template. If this is set, DefaultSolutionNameFormat
    /// is ignored.
    /// </summary>
    [TemplateFieldMetadata("Default Solution Name", PropertyType.String, 0)]
    public string DefaultSolutionName = string.Empty;

    /// <summary>
    /// The default author for a new solution using this template
    /// </summary>
    [TemplateFieldMetadata("Default Starting Version", PropertyType.String, 1)]
    public string DefaultStartingVersion = string.Empty;

    /// <summary>
    /// The directories excluded in prepare
    /// </summary>
    [TemplateFieldMetadata("Prepare-excluded Directories (comma-separated)", PropertyType.StringListComma, 300)]
    public List<string> DirectoriesExcludedInPrepare = [];

    /// <summary>
    /// Manual instructions to display after a new solution using this template has been created
    /// </summary>
    [TemplateFieldMetadata("Instructions (semi-colan-separated)", PropertyType.StringListSemiColan, 401)]
    public List<string> Instructions = [];

    /// <summary>
    /// The license expression
    /// </summary>
    [TemplateFieldMetadata("License Expression", PropertyType.String, 5)]
    [AllowedValue(Constants.LICENSE_EXPRESSIONS, PropertyType.StringListComma)]
    public string LicenseExpression = string.Empty;

    /// <summary>
    /// Files and directories we only rename if need, not edit the contents of, when creating a new solution using
    /// this template
    /// </summary>
    [TemplateFieldMetadata("Rename-only Files and Directories (comma-separated)", PropertyType.StringListComma, 302)]
    public List<string> RenameOnlyFilesAndDirectories = [];

    /// <summary>
    /// Text to replace in the solution's files and directories after a new solution using this template has been created
    /// </summary>
    [TemplateFieldMetadata("Replacement Text (Format: key: value, key: value, etc.)",
        PropertyType.DictionaryStringString, 200)]
    public Dictionary<string, string> ReplacementText = [];

    /// <summary>
    /// The Python scripts to run after a new solution using this template has been created
    /// </summary>
    [TemplateFieldMetadata("Python Script Files (semi-colan-separated)", PropertyType.StringListSemiColan, 400)]
    public List<string> Scripts = [];
}