using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectTools.Core.Helpers;

namespace ProjectTools.Core.Templating.Common
{
    /// <summary>
    /// Information on the template
    /// </summary>
    [DebuggerDisplay("{Name}")]
    [JsonDerivedType(typeof(TemplateInformation), typeDiscriminator: "base")]
    public class TemplateInformation
    {
        /// <summary>
        /// The author of the template
        /// </summary>
        [TemplateFieldMetadata("Template Author", PropertyType.String, order: 1)]
        public string Author = string.Empty;

        /// <summary>
        /// The description of the template
        /// </summary>
        [TemplateFieldMetadata("Template Description", PropertyType.String, order: 3)]
        public string Description = string.Empty;

        /// <summary>
        /// The name of the template
        /// </summary>
        [TemplateFieldMetadata("Template Name", PropertyType.String, order: 0)]
        public string Name = string.Empty;

        /// <summary>
        /// The version of the template
        /// </summary>
        [TemplateFieldMetadata("Template Version", PropertyType.String, order: 2)]
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
