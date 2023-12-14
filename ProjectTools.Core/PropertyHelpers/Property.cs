using System.Diagnostics;

namespace ProjectTools.Core.PropertyHelpers
{
    /// <summary>
    /// Represents the setting for a template.
    /// </summary>
    [DebuggerDisplay("{Name}: {CurrentValue}")]
    public class Property
    {
        /// <summary>
        /// The actual type
        /// </summary>
        public required Type ActualType;

        /// <summary>
        /// The allowed values
        /// </summary>
        public required List<string> AllowedValues;

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

        /// <summary>
        /// Gets the display value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The string representation of the value.</returns>
        public string GetDisplayValue()
        {
            switch (Type)
            {
                case PropertyType.Bool:
                    return (bool)(CurrentValue ?? false) ? "Yes" : "No";

                case PropertyType.String:
                    return (string)(CurrentValue ?? string.Empty);

                case PropertyType.StringListComma:
                    return string.Join(", ", CurrentValue as List<string> ?? []);

                case PropertyType.StringListSemiColan:
                    return string.Join("; ", CurrentValue as List<string> ?? []);

                case PropertyType.DictionaryStringString:
                    var tempDSS = CurrentValue as Dictionary<string, string> ?? [];
                    var tempDSS2 = tempDSS.Select(x => string.Join(": ", x.Key, x.Value));
                    return string.Join(", ", tempDSS2);

                case PropertyType.Enum:
                    return (CurrentValue as Enum)?.ToString() ?? string.Empty;

                default:
                    throw new Exception($"Unknown setting type {Type}!");
            }
        }
    }
}
