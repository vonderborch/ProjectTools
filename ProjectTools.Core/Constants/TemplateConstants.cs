namespace ProjectTools.Core.Constants;

/// <summary>
///     Various constants related to templates.
/// </summary>
public static class TemplateConstants
{
    /// <summary>
    ///     The name of the template settings file.
    /// </summary>
    public static string TemplateSettingsFileName = ".template.json";

    /// <summary>
    ///     The default template repository
    /// </summary>
    public static string DefaultTemplateRepository = "https://github.com/vonderborch/ProjectTools";

    /// <summary>
    ///     The file extension for templates.
    /// </summary>
    public static readonly string TemplateFileExtension = "ptt";

    /// <summary>
    ///     The default template version.
    /// </summary>
    public static Version DefaultTemplateVersion = new(0, 0, 0);

    /// <summary>
    ///     The current template version.
    /// </summary>
    public static Version CurrentTemplateVersion = new(1, 0, 0);

    /// <summary>
    ///     The minimum supported template version.
    /// </summary>
    public static Version MinSupportedTemplateVersion = new(1, 0, 0);

    /// <summary>
    ///     The maximum supported template version.
    /// </summary>
    public static Version MaxSupportedTemplateVersion = new(1, 1, 0);

    /// <summary>
    ///     The maximum depth to search for git repo templates.
    /// </summary>
    public static int MaxGitRepoTemplateSearchDepth = 3;

    /// <summary>
    ///     The excluded file names
    /// </summary>
    public static List<string> GeneratedProjectExcludedFileNames => [TemplateSettingsFileName];

    public static List<string> ExcludedDirectoryNamesDuringProjectGeneration => [".git"];
}
