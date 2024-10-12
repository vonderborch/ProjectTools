using System.Diagnostics;

namespace ProjectTools.CoreOLD.PropertyHelpers
{
    /// <summary>
    /// An attribute to describe an allowed value for this field.
    /// </summary>
    /// <seealso cref="Attribute"/>
    [DebuggerDisplay("{DisplayName}")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AllowedValue : Attribute
    {
        /// <summary>
        /// The type
        /// </summary>
        public readonly PropertyType Type;

        /// <summary>
        /// The default value
        /// </summary>
        public readonly object? Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllowedValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        public AllowedValue(object? value, PropertyType type)
        {
            Value = value;
            Type = type;
        }
    }
}
