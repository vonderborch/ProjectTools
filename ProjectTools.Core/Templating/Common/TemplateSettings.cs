using System.Text.Json.Serialization;
using ProjectTools.Core.Implementations.DotSln;

namespace ProjectTools.Core.Templating.Common
{
    /// <summary>
    /// Settings related to the template
    /// </summary>
    [JsonDerivedType(typeof(TemplateSettings), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(DotSlnTemplateSettings), typeDiscriminator: "dotsln")]
    public class TemplateSettings
    {
        /// <summary>
        /// The files and directories to remove from a new solution using this template
        /// </summary>
        [SettingMetaAttribute("Cleanup Files And Directories (comma-separated)", SettingType.StringListComma, order: 6)]
        public List<string> CleanupFilesAndDirectories =  [ ];

        /// <summary>
        /// The commands to run after a new solution using this template has been created
        /// </summary>
        [SettingMetaAttribute("Commands (semi-colan-separated)", SettingType.StringListSemiColan, order: 8)]
        public List<string> Commands =  [ ];

        /// <summary>
        /// The default author for a new solution using this template
        /// </summary>
        [SettingMetaAttribute("Default Author", SettingType.String, order: 2)]
        public string DefaultAuthor = string.Empty;

        /// <summary>
        /// The default description for a new solution using this template
        /// </summary>
        [SettingMetaAttribute("Default Description", SettingType.String, order: 3)]
        public string DefaultDescription = string.Empty;

        /// <summary>
        /// The default solution name for a new solution using this template. If this is set, DefaultSolutionNameFormat
        /// is ignored.
        /// </summary>
        [SettingMetaAttribute("Default Solution Name", SettingType.String, order: 0)]
        public string DefaultSolutionName = string.Empty;

        /// <summary>
        /// The default solution name format for a new solution using this template
        /// </summary>
        [SettingMetaAttribute("Default Solution Name Format", SettingType.String, order: 1)]
        public string DefaultSolutionNameFormat = string.Empty;

        /// <summary>
        /// The directories excluded in prepare
        /// </summary>
        [SettingMetaAttribute("Prepare-excluded Directories (comma-separated)", SettingType.StringListComma, order: 4)]
        public List<string> DirectoriesExcludedInPrepare =  [ ];

        /// <summary>
        /// Manual instructions to display after a new solution using this template has been created
        /// </summary>
        [SettingMetaAttribute("Instructions (semi-colan-separated)", SettingType.StringListSemiColan, order: 9)]
        public List<string> Instructions =  [ ];

        /// <summary>
        /// Files and directories we only rename if need, not edit the contents of, when creating a new solution using
        /// this template
        /// </summary>
        [SettingMetaAttribute(
            "Rename-only Files and Directories (comma-separated)",
            SettingType.StringListComma,
            order: 5
        )]
        public List<string> RenameOnlyFilesAndDirectories =  [ ];

        /// <summary>
        /// Text to replace in the solution's files and directories after a new solution using this template has been created
        /// </summary>
        [SettingMetaAttribute(
            "Replacement Text (Format: key: value, key: value, etc.)",
            SettingType.DictionaryStringString,
            order: 7
        )]
        public Dictionary<string, string> ReplacementText =  [ ];
    }
}
