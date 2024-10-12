using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;

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
    /// The seconds between template update checks
    /// </summary>
    public int SecondsBetweenTemplateUpdateChecks = 86400;

    /// <summary>
    /// The version of the settings file. This is used to determine if the existing settings file is compatible with
    /// the current version of the application.
    /// </summary>
    public Version SettingsVersion = AppSettingsConstants.SettingsVersion;

    /// <summary>
    /// A list of repositories templates are pulled from.
    /// </summary>
    [JsonIgnore]
    public List<string> RepositoriesList;

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

        var rawContents = File.ReadAllText(AppSettingsConstants.SettingsFilePath);
        if (!string.IsNullOrWhiteSpace(rawContents))
        {
            try
            {
                var settings = JsonSerializer.Deserialize<AppSettings>(rawContents, JsonConstants.JsonSerializeOptions);
                // TODO: Long term, we should change what we do depending on the SettingsVersion stored in the file (upgrade, etc.)
                return settings ?? null;
            }
            catch
            {
                // TODO: Long term, we should log this error...and do something...but for now, just return null
                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// Saves the settings to a file.
    /// </summary>
    public void Save()
    {
        // Save the current settings to the settings file
        var contents = JsonSerializer.Serialize(this, JsonConstants.JsonSerializeOptions);
        _ = Directory.CreateDirectory(Path.GetDirectoryName(AppSettingsConstants.SettingsFilePath));
        File.WriteAllText(AppSettingsConstants.SettingsFilePath, contents);
    }
}
