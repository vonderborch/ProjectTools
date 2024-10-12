using System.Diagnostics;

namespace ProjectTools.CoreOLD.PropertyHelpers
{
    /// <summary>
    /// An attribute to describe the metadata of a user-configurable template field.
    /// </summary>
    /// <seealso cref="Attribute"/>
    [DebuggerDisplay("{DisplayName}")]
    [AttributeUsage(AttributeTargets.Field)]
    public class TemplateFieldMetadata : Attribute
    {
        /// <summary>
        /// The value of the required field that causes this field to be not shown to a user.
        /// </summary>
        public readonly object? DisallowedFieldValue;

        /// <summary>
        /// The display name
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// The display order of the item.
        /// </summary>
        public readonly int Order;

        /// <summary>
        /// The name of another field that is required for this field to be populated
        /// </summary>
        public readonly string? RequiredFieldName;

        /// <summary>
        /// The required value of the other required field.
        /// </summary>
        public readonly object? RequiredFieldValue;

        /// <summary>
        /// The type
        /// </summary>
        public readonly PropertyType Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateFieldMetadata"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="type">The type.</param>
        /// <param name="order">The order.</param>
        /// <param name="requiredFieldName">The name of another field that is required for this field to be populated.</param>
        /// <param name="requiredFieldValue">The required value of the other required field.</param>
        /// <param name="disallowedFieldValue">
        /// The value of the required field that causes this field to be not shown to a user.
        /// </param>
        public TemplateFieldMetadata(string displayName, PropertyType type, int order, string? requiredFieldName = null, object? requiredFieldValue = null, object? disallowedFieldValue = null)
        {
            DisplayName = displayName;
            Type = type;
            Order = order;
            RequiredFieldName = requiredFieldName;
            RequiredFieldValue = requiredFieldValue;
            DisallowedFieldValue = disallowedFieldValue;
        }
    }
}
