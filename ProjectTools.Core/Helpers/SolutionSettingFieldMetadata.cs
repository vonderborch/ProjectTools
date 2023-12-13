using System.Diagnostics;

namespace ProjectTools.Core.Helpers
{
    /// <summary>
    /// An attribute to describe the metadata of a user-configurable template field.
    /// </summary>
    /// <seealso cref="System.Attribute"/>
    [DebuggerDisplay("{DisplayName}")]
    [AttributeUsage(AttributeTargets.Field)]
    public class SolutionSettingFieldMetadata : TemplateFieldMetadata
    {
        /// <summary>
        /// The template setting field name
        /// </summary>
        public readonly string TemplateSettingFieldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionSettingFieldMetadata"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="templateSettingFieldName">Name of the template setting field.</param>
        /// <param name="type">The type.</param>
        /// <param name="order">The order.</param>
        /// <param name="requiredFieldName">Name of the required field.</param>
        /// <param name="requiredFieldValue">The required field value.</param>
        public SolutionSettingFieldMetadata(string displayName, string templateSettingFieldName, PropertyType type, int order, string? requiredFieldName = null, object? requiredFieldValue = null) : base(displayName, type, order, requiredFieldName, requiredFieldValue)
        {
            TemplateSettingFieldName = templateSettingFieldName;
        }
    }
}
