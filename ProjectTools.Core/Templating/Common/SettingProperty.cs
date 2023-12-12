using System.Diagnostics;

namespace ProjectTools.Core.Templating.Common
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
        /// The type
        /// </summary>
        public required SettingType Type;

        /// <summary>
        /// The order
        /// </summary>
        public required int Order;
    }
}
