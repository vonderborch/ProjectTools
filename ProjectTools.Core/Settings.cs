using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectTools.Core
{
    public class Settings
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
        public Version SettingsVersion = new("1.1");

        /// <summary>
        /// A list of repositories templates are pulled from.
        /// </summary>
        public List<string> TemplateRepositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            TemplateRepositories = new();
            LastTemplatesUpdateCheck = DateTime.MinValue;
            GitWebPath = "https://github.com/";
            GitAccessToken = string.Empty;
        }

        /// <summary>
        /// Gets the repositories list text.
        /// </summary>
        /// <value>The repositories list text.</value>
        [JsonIgnore]
        public string RepositoriesListText => string.Join(", ", TemplateRepositories);

        /// <summary>
        /// Gets the secured access token.
        /// </summary>
        /// <value>The secured access token.</value>
        [JsonIgnore]
        public string SecuredAccessToken => GitAccessToken == string.Empty ? string.Empty : "?access_token=****";

        /// <summary>
        /// Gets a value indicating whether [should update templates].
        /// </summary>
        /// <value><c>true</c> if [should update templates]; otherwise, <c>false</c>.</value>
        public bool ShouldUpdateTemplates => (DateTime.Now - LastTemplatesUpdateCheck).TotalSeconds > SecondsBetweenTemplateUpdateChecks;

        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// A Settings object representing the settings stored in the file. Or null if no settings could be loaded.
        /// </returns>
        public static Settings? LoadFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var rawContents = File.ReadAllText(fileName);
            if (!string.IsNullOrWhiteSpace(rawContents))
            {
                try
                {
                    var settings = JsonSerializer.Deserialize<Settings>(rawContents);
                    if (settings == null)
                    {
                        return null;
                    }
                    // TODO: Long term, we should change what we do depending on the SettingsVersion stored in the file (upgrade, etc.)

                    settings.SaveFile(fileName);
                    return settings;
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
        /// Removes any duplicate repositories.
        /// </summary>
        public void RemoveDuplicateRepositories()
        {
            TemplateRepositories = TemplateRepositories.Distinct().ToList();
        }

        /// <summary>
        /// Saves the settings to a file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.ArgumentException">The file name must include a directory. - fileName</exception>
        public void SaveFile(string fileName)
        {
            // Path Validations
            var directory = Path.GetDirectoryName(fileName);
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("The file name must include a directory.", nameof(fileName));
            }

            // Settings Validations
            RemoveDuplicateRepositories();

            // Save the current settings to the settings file
            var contents = JsonSerializer.Serialize(this, Constants.JsonSerializeOptions);
            Directory.CreateDirectory(directory);
            File.WriteAllText(fileName, contents);
        }
    }
}
