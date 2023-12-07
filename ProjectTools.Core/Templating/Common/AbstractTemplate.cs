using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectTools.Core.Implementations.DotSln;
using ProjectTools.Core.Templating.Repositories;

namespace ProjectTools.Core.Templating.Common
{
    /// <summary>
    /// Defines a Project/Solution Template
    /// </summary>
    [JsonDerivedType(typeof(AbstractTemplate), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(DotSlnTemplate), typeDiscriminator: "dotsln")]
    public class AbstractTemplate
    {
        /// <summary>
        /// The author of the template
        /// </summary>
        public string Author = string.Empty;

        /// <summary>
        /// The description of the template
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        /// The file path
        /// </summary>
        [JsonIgnore]
        public string FilePath = string.Empty;

        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// The information on the repo for the template
        /// </summary>
        [JsonIgnore]
        public TemplateGitMetadata? RepoInfo = null;

        /// <summary>
        /// The settings
        /// </summary>
        public TemplateSettings? Settings = null;

        /// <summary>
        /// The version of the template
        /// </summary>
        public string Version = string.Empty;

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
