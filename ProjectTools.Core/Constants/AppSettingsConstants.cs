namespace ProjectTools.Core.Constants;

public static class AppSettingsConstants
{
    /// <summary>
    ///     The file name of the settings file.
    /// </summary>
    public const string SettingsFileName = "settings.json";

    /// <summary>
    ///     The version of the settings file. This is used to determine if the existing settings file is compatible with
    ///     the current version of the application.
    /// </summary>
    public static readonly Version SettingsVersion = new(1, 3);

    /// <summary>
    ///     The default settings version to attempt to load from if all else fails...
    /// </summary>
    public static readonly Version DefaultNotFoundSettingsVersion = new(0, 0, 0);

    /// <summary>
    ///     The path to the settings file.
    /// </summary>
    public static string SettingsFilePath => Path.Combine(PathConstants.CoreDirectory, SettingsFileName);
}
