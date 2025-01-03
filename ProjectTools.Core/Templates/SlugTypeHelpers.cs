namespace ProjectTools.Core.Templates;

/// <summary>
///     Various helpers related to slug types.
/// </summary>
public static class SlugTypeHelpers
{
    /// <summary>
    ///     Slug types that can be null.
    /// </summary>
    private static readonly List<SlugType> NullableSlugTypes =
    [
        SlugType.Boolean,
        SlugType.Int
    ];

    /// <summary>
    ///     Converts the object to a string.
    /// </summary>
    /// <param name="type">The slug type.</param>
    /// <param name="value">The value.</param>
    /// <returns>The string representation of the object.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raises if the slug type isn't supported.</exception>
    public static string ObjectToString(this SlugType type, object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        switch (type)
        {
            case SlugType.Boolean:
            case SlugType.Int:
            case SlugType.RandomGuid:
            case SlugType.String:
                return value.ToString() ?? string.Empty;
            case SlugType.StringListComma:
                return string.Join(",", (List<string>)value);
            case SlugType.StringListSemiColan:
                return string.Join(";", (List<string>)value);
            case SlugType.DictionaryStringString:
                var dict = (Dictionary<string, string>)value;
                return string.Join(", ", dict.Select(x => $"{x.Key}: {x.Value}"));
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    /// <summary>
    ///     Converts the provided value to the correct object type.
    /// </summary>
    /// <param name="type">The type to convert to.</param>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="NotImplementedException">Raises if a particular type isn't supported.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Raises if the type is invalid.</exception>
    public static object? CorrectedValueType(this SlugType type, string value)
    {
        if (string.IsNullOrEmpty(value) && NullableSlugTypes.Contains(type))
        {
            return null;
        }

        switch (type)
        {
            case SlugType.String:
                return value;
            case SlugType.Boolean:
                return bool.Parse(value);
            case SlugType.Int:
                return int.Parse(value);
            case SlugType.StringListComma:
                return ConvertStringList(value, ",");
            case SlugType.StringListSemiColan:
                return ConvertStringList(value, ";");
            case SlugType.DictionaryStringString:
                return ConvertDictionaryStringString(value);
            case SlugType.RandomGuid:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    /// <summary>
    ///     Converts the provided value to the correct object type.
    /// </summary>
    /// <param name="type">The type to convert to.</param>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="NotImplementedException">Raises if a particular type isn't supported.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Raises if the type is invalid.</exception>
    public static List<object?> CorrectedValueType(this SlugType type, List<string> values)
    {
        List<object?> correctedValues = new();
        foreach (var value in values)
        {
            correctedValues.Add(type.CorrectedValueType(value));
        }

        return correctedValues;
    }

    /// <summary>
    ///     Converts a string to a list of values.
    /// </summary>
    /// <param name="value">The value to split.</param>
    /// <param name="separator">The separator.</param>
    /// <returns>The list of values.</returns>
    private static List<string> ConvertStringList(string value, string separator)
    {
        return value.Split(separator).Select(x => x.Trim()).ToList();
    }

    /// <summary>
    ///     Converts a string to a string/string dictionary.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The resulting dictionary.</returns>
    private static Dictionary<string, string> ConvertDictionaryStringString(string value)
    {
        Dictionary<string, string> output = new();
        var baseValues = value.Split(",");
        foreach (var subValue in baseValues)
        {
            var subValues = subValue.Split(":");
            output.Add(subValues[0].Trim(), subValues[1].Trim());
        }

        return output;
    }
}
