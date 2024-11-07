using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectTools.Core.Constants;

/// <summary>
///     Constants related to JSON
/// </summary>
public static class JsonConstants
{
    /// <summary>
    ///     The json serialize options
    /// </summary>
    public static readonly JsonSerializerOptions JsonSerializeOptions = new()
        { WriteIndented = true, Converters = { new JsonStringEnumConverter() }, IncludeFields = true };
}
