using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.Core.Settings;

/// <summary>
///     Settings for the application.
/// </summary>
[SettingRegistration(1, 4, 1, 5)]
public class AppSettings_1_4_0 : AbstractSettings
{
    /// <summary>
    ///     The git sources and their access tokens.
    /// </summary>
    public Dictionary<string, string> GitSourcesAndAccessTokens;

    /// <summary>
    ///     The last time the template repositories were checked for updates.
    /// </summary>
    public DateTime LastTemplatesUpdateCheck;

    /// <summary>
    ///     A dictionary representing repositories and their respective git source.
    /// </summary>
    public Dictionary<string, string> RepositoriesAndGitSources;

    /// <summary>
    ///     The seconds between template update checks
    /// </summary>
    public int SecondsBetweenTemplateUpdateChecks = 86400;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppSettings" /> class.
    /// </summary>
    public AppSettings_1_4_0()
    {
        this.GitSourcesAndAccessTokens = [];
        this.RepositoriesAndGitSources = [];
        this.LastTemplatesUpdateCheck = DateTime.MinValue;
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
        var settings = JsonHelpers.DeserializeFromFile<AppSettings_1_4_0>(AppSettingsConstants.SettingsFilePath);
        return settings;
    }

    /// <summary>
    ///     A method to convert the settings to the next version.
    /// </summary>
    /// <returns>The updated settings.</returns>
    public new static AbstractSettings? ToNextSettingsVersion(AbstractSettings? currentSettings)
    {
        if (currentSettings == null)
        {
            return null;
        }

        var actualCurrentSettings = (AppSettings_1_4_0)currentSettings;
        AppSettings nextVersion = new() // Update AppSettings -> AppSettings_1_5_0
        {
            LastTemplatesUpdateCheck = actualCurrentSettings.LastTemplatesUpdateCheck,
            SecondsBetweenTemplateUpdateChecks = actualCurrentSettings.SecondsBetweenTemplateUpdateChecks,
            GitSourcesAndAccessTokens = actualCurrentSettings.GitSourcesAndAccessTokens,
            RepositoriesAndGitSources = actualCurrentSettings.RepositoriesAndGitSources,
            PythonVersion = PythonConstants.PythonVersion
        };

        return nextVersion;
    }
}
