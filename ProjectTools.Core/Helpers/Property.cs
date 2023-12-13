using System.Diagnostics;

namespace ProjectTools.Core.Helpers
{
    /// <summary>
    /// Represents the setting for a template.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class Property
    {
        /// <summary>
        /// The current value
        /// </summary>
        public required object? CurrentValue;

        /// <summary>
        /// The display name
        /// </summary>
        public required string DisplayName;

        /// <summary>
        /// Metadata on the property
        /// </summary>
        public required TemplateFieldMetadata Metadata;

        /// <summary>
        /// The name
        /// </summary>
        public required string Name;

        /// <summary>
        /// The order
        /// </summary>
        public required int Order;

        /// <summary>
        /// The type
        /// </summary>
        public required PropertyType Type;

        /// <summary>
        /// Gets the setting source.
        /// </summary>
        /// <value>The setting source.</value>
        public PropertySource SettingSource { get; internal set; }
    }
}
