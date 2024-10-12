namespace ProjectTools.Core.Constants;

public static class AppSettingsConstants
{
    /// <summary>
    /// The version of the settings file. This is used to determine if the existing settings file is compatible with
    /// the current version of the application.
    /// </summary>
    public const Version SettingsVersion = new("1.1");

    /// <summary>
    /// The file name of the settings file.
    /// </summary>
    public const string SettingsFileName = "settings.json";

    /// <summary>
    /// The path to the settings file.
    /// </summary>
    public static string SettingsFilePath => Path.Combine(DirectoryConstants.CoreDirectory, SettingsFileName);

    /// <summary>
    /// The default template repository
    /// </summary>
    public static string DefaultRepository = "https://github.com/vonderborch/ProjectTools";
}
