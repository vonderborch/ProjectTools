using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.Core;

/// <summary>
/// Settings for the application.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// The git access token
    /// </summary>
    public string GitAccessToken;

    /// <summary>
    /// The git web path
    /// </summary>
    public string GitWebPath;

    /// <summary>
    /// The last time the template repositories were checked for updates.
    /// </summary>
    public DateTime LastTemplatesUpdateCheck;

    /// <summary>
    /// A list of repositories templates are pulled from.
    /// </summary>
    [JsonIgnore] public List<string> RepositoriesList;

    /// <summary>
    /// The seconds between template update checks
    /// </summary>
    public int SecondsBetweenTemplateUpdateChecks = 86400;

    /// <summary>
    /// The version of the settings file. This is used to determine if the existing settings file is compatible with
    /// the current version of the application.
    /// </summary>
    public Version SettingsVersion = AppSettingsConstants.SettingsVersion;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppSettings"/> class.
    /// </summary>
    public AppSettings()
    {
        RepositoriesList = [];
        LastTemplatesUpdateCheck = DateTime.MinValue;
        GitWebPath = "https://github.com/";
        GitAccessToken = string.Empty;
    }

    /// <summary>
    /// Gets the repositories list text.
    /// </summary>
    /// <value>The repositories list text.</value>
    [JsonIgnore]
    public string RepositoriesListText => string.Join(", ", RepositoriesList);

    /// <summary>
    /// Gets the secured access token.
    /// </summary>
    /// <value>The secured access token.</value>
    [JsonIgnore]
    public string SecuredAccessToken =>
        GitAccessToken == string.Empty ? string.Empty : "?access_token=****";

    /// <summary>
    /// Gets a value indicating whether [should update templates].
    /// </summary>
    /// <value><c>true</c> if [should update templates]; otherwise, <c>false</c>.</value>
    [JsonIgnore]
    public bool ShouldUpdateTemplates =>
        (DateTime.Now - LastTemplatesUpdateCheck).TotalSeconds
        > SecondsBetweenTemplateUpdateChecks;

    /// <summary>
    /// Adds a template repository.
    /// </summary>
    /// <param name="repository">The new template repository to add.</param>
    public void AddTemplateRepository(string repository)
    {
        if (!RepositoriesList.Contains(repository))
        {
            RepositoriesList.Add(repository);
        }
    }

    /// <summary>
    /// Loads the file.
    /// </summary>
    /// <returns>
    /// A Settings object representing the settings stored in the file. Or null if no settings could be loaded.
    /// </returns>
    public static AppSettings? Load()
    {
        if (!File.Exists(AppSettingsConstants.SettingsFilePath))
        {
            return null;
        }

        var settings = JsonHelpers.DeserializeFromFile<AppSettings>(AppSettingsConstants.SettingsFilePath);
        if (settings == null)
        {
            // TODO: Long term, we should change what we do depending on the SettingsVersion stored in the file (upgrade, etc.)
        }

        return settings;
    }

    /// <summary>
    /// Saves the settings to a file.
    /// </summary>
    public void Save()
    {
        JsonHelpers.SerializeToFile(AppSettingsConstants.SettingsFilePath, this);
    }
}
