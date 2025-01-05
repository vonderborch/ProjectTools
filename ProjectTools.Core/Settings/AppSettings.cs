using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.Core.Settings;

/// <summary>
///     Settings for the application.
/// </summary>
[SettingRegistration(1, 6)]
public class AppSettings : AbstractSettings
{
    /// <summary>
    ///     The git sources and their access tokens.
    /// </summary>
    public Dictionary<string, string> GitSourcesAndAccessTokens;

    /// <summary>
    ///     The last time the app was checked for updates.
    /// </summary>
    public Dictionary<string, DateTime> LastAppUpdateCheck;

    /// <summary>
    ///     The last time the template repositories were checked for updates.
    /// </summary>
    public DateTime LastTemplatesUpdateCheck;

    /// <summary>
    ///     The version of Python being used
    /// </summary>
    public string PythonVersion = PythonConstants.PythonVersion;

    /// <summary>
    ///     A dictionary representing repositories and their respective git source.
    /// </summary>
    public Dictionary<string, string> RepositoriesAndGitSources;

    /// <summary>
    ///     The seconds between app update checks.
    /// </summary>
    public int SecondsBetweenAppUpdateChecks = 86400;

    /// <summary>
    ///     The seconds between template update checks
    /// </summary>
    public int SecondsBetweenTemplateUpdateChecks = 86400;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppSettings" /> class.
    /// </summary>
    public AppSettings()
    {
        this.GitSourcesAndAccessTokens = [];
        this.RepositoriesAndGitSources = [];
        this.LastTemplatesUpdateCheck = DateTime.MinValue;
        this.LastAppUpdateCheck = new Dictionary<string, DateTime>
        {
            { AppConstants.AppNameCommandLine, DateTime.MinValue },
            { AppConstants.AppNameGui, DateTime.MinValue }
        };
    }

    /// <summary>
    ///     The list of repositories.
    /// </summary>
    [JsonIgnore]
    public List<string> RepositoriesList => this.RepositoriesAndGitSources.Keys.ToList();

    /// <summary>
    ///     Gets a value indicating whether [should update templates].
    /// </summary>
    /// <value><c>true</c> if [should update templates]; otherwise, <c>false</c>.</value>
    [JsonIgnore]
    public bool ShouldUpdateTemplates =>
        (DateTime.Now - this.LastTemplatesUpdateCheck).TotalSeconds
        > this.SecondsBetweenTemplateUpdateChecks;

    /// <summary>
    ///     Loads the settings file.
    /// </summary>
    /// <returns>The loaded settings class.</returns>
    public new static AbstractSettings? LoadVersion()
    {
        var settings = JsonHelpers.DeserializeFromFile<AppSettings>(AppSettingsConstants.SettingsFilePath);
        return settings;
    }

    /// <summary>
    ///     Gets a value indicating whether we should check for app updates.
    /// </summary>
    /// <param name="appName">The app name to check.</param>
    /// <returns>True if we should check for updates, False otherwise.</returns>
    public bool ShouldCheckForAppUpdates(string appName)
    {
        if (this.LastAppUpdateCheck.TryGetValue(appName, out var lastCheck))
        {
            return (DateTime.Now - lastCheck).TotalSeconds > this.SecondsBetweenAppUpdateChecks;
        }

        return true;
    }

    /// <summary>
    ///     A method to validate the settings.
    /// </summary>
    protected override void ValidateSettings()
    {
        base.ValidateSettings();
        if (!this.GitSourcesAndAccessTokens.TryGetValue("https://github.com/", out _))
        {
            throw new Exception("Error: No GitHub access token found!");
        }
    }

    /// <summary>
    ///     A method to convert the settings to the next version.
    /// </summary>
    /// <returns>The updated settings.</returns>
    public new static AbstractSettings? ToNextSettingsVersion(AbstractSettings? currentSettings)
    {
        throw new NotImplementedException();
    }
}
