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
        /// The type
        /// </summary>
        public readonly SettingType Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingMetaAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="type">The type.</param>
        public SettingMetaAttribute(string displayName, SettingType type)
        {
            DisplayName = displayName;
            Type = type;
        }
    }
}
