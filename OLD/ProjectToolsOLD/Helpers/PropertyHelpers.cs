using System.Text;
using ProjectToolsOLD.CoreOLD.PropertyHelpers;

namespace ProjectToolsOLD.Helpers
{
    /// <summary>
    /// Helpers for setting properties.
    /// </summary>
    public static class PropertyHelpers
    {
        /// <summary>
        /// Continues the editing settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>True to edit, False to finish editing</returns>
        public static bool ContinueEditingSettings(List<Property> settings, Func<string, bool> LogMessage)
        {
            var displayedSettings = settings.Select(x => (x.Order, $"{x.DisplayName}: {GetDisplayValue(x.CurrentValue, x.Type)}{Environment.NewLine}")).OrderBy(x => x.Order).ToList();

            var sb = new StringBuilder();
            foreach (var setting in displayedSettings)
            {
                _ = sb.Append($"    {setting.Item2}");
            }

            var result = ConsoleHelpers.GetYesNo($"Edit settings?{Environment.NewLine}{sb}{Environment.NewLine}", false);
            _ = LogMessage(" ");
            return result;
        }

        /// <summary>
        /// Gets the display value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The string representation of the value.</returns>
        public static string GetDisplayValue(object? value, PropertyType type)
        {
            switch (type)
            {
                case PropertyType.Bool:
                    return (bool)(value ?? false) ? "Yes" : "No";

                case PropertyType.String:
                    return (string)(value ?? string.Empty);

                case PropertyType.StringListComma:
                    return string.Join(", ", (value as List<string>) ?? []);

                case PropertyType.StringListSemiColan:
                    return string.Join("; ", (value as List<string>) ?? []);

                case PropertyType.DictionaryStringString:
                    var tempDSS = (value as Dictionary<string, string>) ?? [];
                    var tempDSS2 = tempDSS.Select(x => string.Join(": ", x.Key, x.Value));
                    return string.Join(", ", tempDSS2);

                case PropertyType.Enum:
                    return (value as Enum)?.ToString() ?? string.Empty;

                default:a
                    throw new Exception($"Unknown setting type {type}!");
            }
        }

        /// <summary>
        /// Gets the input for property.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>The user-entered value for the property</returns>
        /// <exception cref="Exception">$"Unknown setting type {setting.Type}!</exception>
        public static object GetInputForProperty(Property setting)
        {
            object response;
            string tempResponse;
            List<string> tempListResponse;
            var defaultValue = GetDisplayValue(setting.CurrentValue, setting.Type);
            switch (setting.Type)
            {
                case PropertyType.Bool:
                    response = ConsoleHelpers.GetYesNo(setting.DisplayName, defaultValue == "Yes");
                    break;

                case PropertyType.String:
                    if (setting.AllowedValues.Count > 0)
                    {
                        response = ConsoleHelpers.GetInputWithLimit(setting.DisplayName, setting.AllowedValues, defaultValue);
                    }
                    else
                    {
                        response = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    }
                    break;

                case PropertyType.StringListComma:
                    tempResponse = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    tempListResponse = tempResponse.Split(",").Select(x => x.Trim()).ToList();
                    response = tempListResponse.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                    break;

                case PropertyType.StringListSemiColan:
                    tempResponse = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    tempListResponse = tempResponse.Split(";").Select(x => x.Trim()).ToList();
                    response = tempListResponse.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                    break;

                case PropertyType.DictionaryStringString:
                    tempResponse = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    tempListResponse = tempResponse.Split(",").Select(x => x.Trim()).ToList();
                    response = tempListResponse.Where(x => !string.IsNullOrWhiteSpace(x)).ToDictionary(x => x.Split(":")[0].Trim(), x => x.Split(":")[1].Trim());
                    break;

                case PropertyType.Enum:
                    tempListResponse = Enum.GetNames(setting.ActualType).ToList();
                    tempResponse = ConsoleHelpers.GetInputWithLimit(setting.DisplayName, tempListResponse, defaultValue);
                    response = Enum.Parse(setting.ActualType, tempResponse);
                    break;

                default:
                    throw new Exception($"Unknown setting type {setting.Type}!");
            }

            return response;
        }
    }
}
