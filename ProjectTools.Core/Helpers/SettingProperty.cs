using System.Diagnostics;

namespace ProjectTools.Core.Helpers
{
    /// <summary>
    /// Represents the setting for a template.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class SettingProperty
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
        public required SettingType Type;

        /// <summary>
        /// Gets a value indicating whether [from template information class].
        /// </summary>
        /// <value><c>true</c> if [from template information class]; otherwise, <c>false</c>.</value>
        public bool FromTemplateInformationClass { get; internal set; }
    }
}
