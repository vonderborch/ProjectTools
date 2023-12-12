using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectTools.Core
{
    /// <summary>
    /// Constants used throughout the program
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The name of the application
        /// </summary>
        public static readonly string ApplicationName = "ProjectTools";

        /// <summary>
        /// The core directory for the program
        /// </summary>
        public static readonly string CoreDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ProjectTools"
                                                                  );

        /// <summary>
        /// The unique identifier padding
        /// </summary>
        public static readonly string GUID_PADDING = $"D{GUID_PADDING_LENGTH}";

        /// <summary>
        /// The json serialize options
        /// </summary>
        public static readonly JsonSerializerOptions JsonSerializeOptions =
            new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() }, IncludeFields = true };

        /// <summary>
        /// The regex tags
        /// NOTE: Keep in sync with GenerateOptions().UpdateReplacementTextWithTags()
        /// </summary>
        public static readonly string[] REGEX_TAGS =
        {
            "[AUTHOR]",
            "[COMPANY]",
            "[TAGS]",
            "[DESCRIPTION]",
            "[LICENSE]",
            "[VERSION]",
        };

        /// <summary>
        /// The settings file
        /// </summary>
        public static readonly string SettingsFile = Path.Combine(CoreDirectory, "settings.json");

        /// <summary>
        /// The file type templates use (actually a zip file though)
        /// </summary>
        public static readonly string TemplateFileType = "ptt";

        /// <summary>
        /// The file used to store the version info for the downloaded templates
        /// </summary>
        public static readonly string TemplatesCacheFile = Path.Combine(
            CoreDirectory,
            "templates_cache.json"
                                                                       );

        /// <summary>
        /// The directory project templates are stored in
        /// </summary>
        public static readonly string TemplatesDirectory = Path.Combine(CoreDirectory, "Templates");

        /// <summary>
        /// The directory used to store project configuration settings files
        /// </summary>
        public static readonly string TemplatesProjectConfigurationDirectory = Path.Combine(
            TemplatesDirectory,
            "ProjectConfiguration"
                                                                                           );

        /// <summary>
        /// The working file for storing a project's configuration
        /// </summary>
        public static readonly string TemplatesProjectConfigurationFile = Path.Combine(
            CoreDirectory,
            "project_configuration.json"
                                                                                      );

        /// <summary>
        /// The application repository URL
        /// </summary>
        public static string ApplicationRepositoryUrl = "https://github.com/vonderborch/ProjectTools";

        /// <summary>
        /// The default template repository
        /// </summary>
        public static string DefaultTemplateRepository = "https://github.com/vonderborch/ProjectTools";

        /// <summary>
        /// The maximum git repo template search depth
        /// </summary>
        public static int MaxGitRepoTemplateSearchDepth = 1;

        /// <summary>
        /// Name of the specialtext project.
        /// </summary>
        public static string SpecialTextProjectName = "<SolutionName>";

        /// <summary>
        /// The templater templates information file name
        /// </summary>
        public static string TemplaterTemplatesInfoFileName = "template_info.json";

        /// <summary>
        /// The templater templates information file name
        /// </summary>
        public static string TemplaterTemplatesSettingsFileName = "template_settings.json";

        /// <summary>
        /// The unique identifier padding length
        /// </summary>
        private const int GUID_PADDING_LENGTH = 9;
    }
}
