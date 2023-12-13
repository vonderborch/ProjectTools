using System.Diagnostics;

namespace ProjectTools.Core.Helpers
{
    /// <summary>
    /// An attribute to describe the metadata of a user-configurable template field.
    /// </summary>
    /// <seealso cref="System.Attribute"/>
    [DebuggerDisplay("{DisplayName}")]
    [AttributeUsage(AttributeTargets.Field)]
    public class SettingMetadata : Attribute
    {
        /// <summary>
        /// The display name
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// The display order of the item.
        /// </summary>
        public readonly int Order;

        /// <summary>
        /// The type
        /// </summary>
        public readonly SettingType Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingMetadata"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="type">The type.</param>
        public SettingMetadata(string displayName, SettingType type, int order)
        {
            DisplayName = displayName;
            Type = type;
            Order = order;
        }
    }
}
