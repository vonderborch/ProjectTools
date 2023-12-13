using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Implementations.DotSln;

namespace ProjectTools.Core.Templating.Common
{
    /// <summary>
    /// Information on template settings
    /// </summary>
    [DebuggerDisplay("{DefaultSolutionName}")]
    [JsonDerivedType(typeof(TemplateSettings), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(DotSlnTemplateSettings), typeDiscriminator: "dotsln")]
    public class TemplateSettings
    {
        /// <summary>
        /// The files and directories to remove from a new solution using this template
        /// </summary>
        [TemplateFieldMetadata("Cleanup Files And Directories (comma-separated)", PropertyType.StringListComma, order: 301)]
        public List<string> CleanupFilesAndDirectories = [];

        /// <summary>
        /// The commands to run after a new solution using this template has been created
        /// </summary>
        [TemplateFieldMetadata("Commands (semi-colan-separated)", PropertyType.StringListSemiColan, order: 400)]
        public List<string> Commands = [];

        /// <summary>
        /// The default author for a new solution using this template
        /// </summary>
        [TemplateFieldMetadata("Default Author", PropertyType.String, order: 2)]
        public string DefaultAuthor = string.Empty;

        /// <summary>
        /// The default description for a new solution using this template
        /// </summary>
        [TemplateFieldMetadata("Default Description", PropertyType.String, order: 10)]
        public string DefaultDescription = string.Empty;

        /// <summary>
        /// The default solution name for a new solution using this template. If this is set, DefaultSolutionNameFormat
        /// is ignored.
        /// </summary>
        [TemplateFieldMetadata("Default Solution Name", PropertyType.String, order: 0)]
        public string DefaultSolutionName = string.Empty;

        /// <summary>
        /// The default author for a new solution using this template
        /// </summary>
        [TemplateFieldMetadata("Default Starting Version", PropertyType.String, order: 1)]
        public string DefaultStartingVersion = string.Empty;

        /// <summary>
        /// The directories excluded in prepare
        /// </summary>
        [TemplateFieldMetadata("Prepare-excluded Directories (comma-separated)", PropertyType.StringListComma, order: 300)]
        public List<string> DirectoriesExcludedInPrepare = [];

        /// <summary>
        /// Manual instructions to display after a new solution using this template has been created
        /// </summary>
        [TemplateFieldMetadata("Instructions (semi-colan-separated)", PropertyType.StringListSemiColan, order: 401)]
        public List<string> Instructions = [];

        /// <summary>
        /// Files and directories we only rename if need, not edit the contents of, when creating a new solution using
        /// this template
        /// </summary>
        [TemplateFieldMetadata("Rename-only Files and Directories (comma-separated)", PropertyType.StringListComma, order: 302)]
        public List<string> RenameOnlyFilesAndDirectories = [];

        /// <summary>
        /// Text to replace in the solution's files and directories after a new solution using this template has been created
        /// </summary>
        [TemplateFieldMetadata("Replacement Text (Format: key: value, key: value, etc.)", PropertyType.DictionaryStringString, order: 200)]
        public Dictionary<string, string> ReplacementText = [];

        /// <summary>
        /// Converts the current instance to a JSON string.
        /// </summary>
        /// <returns>The JSON string.</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, Constants.JsonSerializeOptions);
        }
    }
}
