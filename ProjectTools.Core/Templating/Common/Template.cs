using System.Diagnostics;

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
    }
}
