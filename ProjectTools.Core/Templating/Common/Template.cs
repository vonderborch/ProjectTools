using System.Diagnostics;
using System.Text.Json;

namespace ProjectTools.Core.Templating.Common
{
    /// <summary>
    /// Information on a template
    /// </summary>
    [DebuggerDisplay("{Information}")]
    public class Template
    {
        /// <summary>
        /// The information
        /// </summary>
        public required TemplateInformation Information;

        /// <summary>
        /// The settings
        /// </summary>
        public required TemplateSettings Settings;

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
