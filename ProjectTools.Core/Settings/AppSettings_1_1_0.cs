using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.Core.Settings;

/// <summary>
///     Settings for the application.
/// </summary>
[SettingRegistration(1, 1, 1, 2)]
public class AppSettings_1_1_0 : AbstractSettings
{
    /// <summary>
    ///     The git access token
    /// </summary>
    public string GitAccessToken;

    /// <summary>
    ///     The git web path
    /// </summary>
    public string GitWebPath;

    /// <summary>
    ///     The last time the template repositories were checked for updates.
    /// </summary>
    public DateTime LastTemplatesUpdateCheck;

    /// <summary>
    ///     The seconds between template update checks
    /// </summary>
    public int SecondsBetweenTemplateUpdateChecks = 86400;

    /// <summary>
    ///     A list of repositories templates are pulled from.
    /// </summary>
    public List<string> TemplateRepositories;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppSettings_1_1_0" /> class.
    /// </summary>
    public AppSettings_1_1_0()
    {
        this.TemplateRepositories = [];
        this.LastTemplatesUpdateCheck = DateTime.MinValue;
        this.GitWebPath = "https://github.com/";
        this.GitAccessToken = string.Empty;
    }

    /// <summary>
    ///     Gets the repositories list text.
    /// </summary>
    /// <value>The repositories list text.</value>
    [JsonIgnore]
    public string RepositoriesListText => string.Join(", ", this.TemplateRepositories);

    /// <summary>
    ///     Gets the secured access token.
    /// </summary>
    /// <value>The secured access token.</value>
    [JsonIgnore]
    public string SecuredAccessToken => this.GitAccessToken == string.Empty ? string.Empty : "?access_token=****";

    /// <summary>
    ///     Gets a value indicating whether [should update templates].
    /// </summary>
    /// <value><c>true</c> if [should update templates]; otherwise, <c>false</c>.</value>
    [JsonIgnore]
    public bool ShouldUpdateTemplates =>
        (DateTime.Now - this.LastTemplatesUpdateCheck).TotalSeconds
        > this.SecondsBetweenTemplateUpdateChecks;

    /// <summary>
    ///     Adds a template repository.
    /// </summary>
    /// <param name="repository">The new template repository to add.</param>
    public void AddTemplateRepository(string repository)
    {
        if (!this.TemplateRepositories.Contains(repository))
        {
            this.TemplateRepositories.Add(repository);
        }
    }

    /// <summary>
    ///     Loads the settings file.
    /// </summary>
    /// <returns>The loaded settings class.</returns>
    public new static AbstractSettings? LoadVersion()
    {
        var settings = JsonHelpers.DeserializeFromFile<AppSettings_1_1_0>(AppSettingsConstants.SettingsFilePath);
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

        var actualCurrentSettings = (AppSettings_1_1_0)currentSettings;
        AppSettings nextVersion = new(); // TODO: Update this to point to 1.2.0 when 1.3.0 is created..
        nextVersion.LastTemplatesUpdateCheck = actualCurrentSettings.LastTemplatesUpdateCheck;
        nextVersion.SecondsBetweenTemplateUpdateChecks = actualCurrentSettings.SecondsBetweenTemplateUpdateChecks;

        nextVersion.GitSources = new Dictionary<string, string>
        {
            [actualCurrentSettings.GitWebPath] = actualCurrentSettings.GitAccessToken
        };

        nextVersion.RepositoriesList = new Dictionary<string, string>();
        foreach (var repo in actualCurrentSettings.TemplateRepositories)
        {
            nextVersion.RepositoriesList.Add(actualCurrentSettings.GitWebPath, repo);
        }

        return nextVersion;
    }
}
