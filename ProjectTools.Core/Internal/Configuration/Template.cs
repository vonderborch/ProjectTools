using ProjectTools.Core.Internal.Repositories;
using System.Text.Json.Serialization;

namespace ProjectTools.Core.Internal.Configuration
{
    /// <summary>
    /// Defines a Project/Solution Template
    /// </summary>
    public class Template
    {
        /// <summary>
        /// The author of the template
        /// </summary>
        public string Author;

        /// <summary>
        /// The description of the template
        /// </summary>
        public string Description;

        /// <summary>
        /// The file path
        /// </summary>
        [JsonIgnore]
        public string FilePath;

        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name;

        /// <summary>
        /// The information on the repo for the template
        /// </summary>
        [JsonIgnore]
        public TemplateGitInfo RepoInfo;

        /// <summary>
        /// The settings
        /// </summary>
        public TemplateSettings Settings;

        /// <summary>
        /// The version of the template
        /// </summary>
        public string Version;
    }
}
