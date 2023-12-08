namespace ProjectTools.Core.Templating.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class SettingMetaAttribute : Attribute
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
        /// Initializes a new instance of the <see cref="SettingMetaAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="type">The type.</param>
        public SettingMetaAttribute(string displayName, SettingType type, int order)
        {
            DisplayName = displayName;
            Type = type;
            Order = order;
        }
    }
}
