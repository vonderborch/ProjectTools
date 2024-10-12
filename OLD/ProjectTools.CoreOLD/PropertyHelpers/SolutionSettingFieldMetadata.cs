using System.Diagnostics;

namespace ProjectTools.CoreOLD.PropertyHelpers
{
    /// <summary>
    /// An attribute to describe the metadata of a user-configurable template field.
    /// </summary>
    /// <seealso cref="Attribute"/>
    [DebuggerDisplay("{DisplayName}")]
    [AttributeUsage(AttributeTargets.Field)]
    public class SolutionSettingFieldMetadata : TemplateFieldMetadata
    {
        /// <summary>
        /// The default value
        /// </summary>
        public readonly object? DefaultValue;

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
        /// <param name="defaultValue">The default value of the field.</param>
        /// <param name="requiredFieldName">Name of the required field.</param>
        /// <param name="requiredFieldValue">The required field value.</param>
        /// <param name="disallowedFieldValue">The disallowed field value.</param>
        public SolutionSettingFieldMetadata(string displayName, string templateSettingFieldName, PropertyType type, int order, object? defaultValue = null, string? requiredFieldName = null, object? requiredFieldValue = null, object? disallowedFieldValue = null) : base(displayName, type, order, requiredFieldName, requiredFieldValue, disallowedFieldValue)
        {
            TemplateSettingFieldName = templateSettingFieldName;
            DefaultValue = defaultValue;
        }
    }
}
