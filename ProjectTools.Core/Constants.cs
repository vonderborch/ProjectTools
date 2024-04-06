using System.Reflection;
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
        /// The possible license expressions
        /// TODO: Keep in sync with https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/licensing-a-repository
        /// </summary>
        public const string LICENSE_EXPRESSIONS = "AFL-3.0, Apache-2.0, Artistic-2.0, BSL-1.0, BSD-2-Clause, BSD-3-Clause, BSD-3-Clause-Clear, BSD-4-Clause, 0BSD, CC, CC0-1.0, CC-BY-4.0, CC-BY-SA-4.0, WTFPL, ECL-2.0, EPL-1.0, EPL-2.0, EUPL-1.1, AGPL-3.0, GPL, GPL-2.0, GPL-3.0, LGPL, LGPL-2.1, LGPL-3.0, ISC, LPPL-1.3c, MS-PL, MIT, MPL-2.0, OSL-3.0, PostgreSQL, OFL-1.1, NCSA, Unlicense, Zlib";

        /// <summary>
        /// The name of the application
        /// </summary>
        public static readonly string ApplicationName = "ProjectTools";

        /// <summary>
        /// The core directory for the program
        /// </summary>
        public static readonly string CoreDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProjectTools");

        /// <summary>
        /// The unique identifier padding
        /// </summary>
        public static readonly string GUID_PADDING = $"D{GUID_PADDING_LENGTH}";

        /// <summary>
        /// The json serialize options
        /// </summary>
        public static readonly JsonSerializerOptions JsonSerializeOptions = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() }, IncludeFields = true };

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
        /// The special text
        /// </summary>
        public static readonly string[] SPECIAL_TEXT = { "<CurrentUserName>", "<CurrentDir>", "<SolutionName>", "<ProjectFullName>" };

        /// <summary>
        /// The file type templates use (actually a zip file though)
        /// </summary>
        public static readonly string TemplateFileType = "ptt";

        /// <summary>
        /// The file used to store the version info for the downloaded templates
        /// </summary>
        public static readonly string TemplatesCacheFile = Path.Combine(CoreDirectory, "templates_cache.json");

        /// <summary>
        /// The directory project templates are stored in
        /// </summary>
        public static readonly string TemplatesDirectory = Path.Combine(CoreDirectory, "Templates");

        /// <summary>
        /// The directory used to store project configuration settings files
        /// </summary>
        public static readonly string TemplatesProjectConfigurationDirectory = Path.Combine(CoreDirectory, "ProjectConfiguration");

        /// <summary>
        /// The working file for storing a project's configuration
        /// </summary>
        public static readonly string TemplatesProjectConfigurationFile = Path.Combine(CoreDirectory, "project_configuration.json");

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
        public static int MaxGitRepoTemplateSearchDepth = 3;

        /// <summary>
        /// Name of the specialtext project.
        /// </summary>
        public static string SpecialTextProjectName = "<SolutionName>";

        /// <summary>
        /// The templater templates information file name
        /// </summary>
        public static string TemplaterTemplatesInfoFileName = "template_info.json";

        /// <summary>
        /// The unique identifier padding length
        /// </summary>
        private const int GUID_PADDING_LENGTH = 9;

        /// <summary>
        /// The excluded files
        /// </summary>
        public static string[] EXCLUDED_FILES => [TemplaterTemplatesInfoFileName];
        
        /// <summary>
        /// The version of the core library
        /// </summary>
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
    }
}
