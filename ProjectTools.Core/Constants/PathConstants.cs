namespace ProjectTools.Core.Constants;

/// <summary>
///     Constants related to directories.
/// </summary>
public static class PathConstants
{
    /// <summary>
    ///     The core directory for the program.
    /// </summary>
    public static readonly string CoreDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProjectTools");

    /// <summary>
    ///     The directory to store local templates in.
    /// </summary>
    public static readonly string TemplateDirectory = Path.Combine(CoreDirectory, "Templates");

    /// <summary>
    ///     The directory to store project generation configs in.
    /// </summary>
    public static readonly string ProjectGenerationConfigDirectory =
        Path.Combine(CoreDirectory, "ProjectGenerationConfigs");

    /// <summary>
    ///     The filename of the file storing information on downloaded and local templates.
    /// </summary>
    public static string TemplatesInfoCacheFileName = "templates_cache.json";

    /// <summary>
    ///     The default project generation config file name.
    /// </summary>
    public static string DefaultProjectGenerationConfigFileName = "latest_project_generation_config.json";

    /// <summary>
    ///     The file storing information on downloaded and local templates.
    /// </summary>
    public static string TemplatesInfoCacheFile => Path.Combine(CoreDirectory, TemplatesInfoCacheFileName);
}
