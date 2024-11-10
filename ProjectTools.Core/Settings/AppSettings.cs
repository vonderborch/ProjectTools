using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.Core.Settings;

/// <summary>
///     Settings for the application.
/// </summary>
[SettingRegistration(1, 3)]
public class AppSettings : AbstractSettings
{
    /// <summary>
    ///     The git sources.
    /// </summary>
    public Dictionary<string, string> GitSources;

    /// <summary>
    ///     The last time the template repositories were checked for updates.
    /// </summary>
    public DateTime LastTemplatesUpdateCheck;

    /// <summary>
    ///     A list of repositories templates are pulled from.
    /// </summary>
    public Dictionary<string, string> RepositoriesList;

    /// <summary>
    ///     The seconds between template update checks
    /// </summary>
    public int SecondsBetweenTemplateUpdateChecks = 86400;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppSettings" /> class.
    /// </summary>
    public AppSettings()
    {
        this.GitSources = [];
        this.RepositoriesList = [];
        this.LastTemplatesUpdateCheck = DateTime.MinValue;
    }

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
    ///     A method to convert the settings to the next version.
    /// </summary>
    /// <returns>The updated settings.</returns>
    public new static AbstractSettings? ToNextSettingsVersion(AbstractSettings? currentSettings)
    {
        throw new NotImplementedException();
    }
}
