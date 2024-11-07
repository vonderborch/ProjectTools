using System.Text.Json;
using ProjectTools.Core.Constants;

namespace ProjectTools.Core.Helpers;

public static class JsonHelpers
{
    /// <summary>
    ///     Deserializes a JSON file to the specified type.
    /// </summary>
    /// <param name="path">The path to the JSON file.</param>
    /// <param name="options">Options to use when deserializing the file.</param>
    /// <typeparam name="TValue">The expected type.</typeparam>
    /// <returns>The type, or the default for the type.</returns>
    public static TValue? DeserializeFromFile<TValue>(string path, JsonSerializerOptions? options = null)
    {
        if (!File.Exists(path))
        {
            return default;
        }

        var rawContents = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(rawContents))
        {
            return default;
        }

        return DeserializeString<TValue>(rawContents, options);
    }

    /// <summary>
    ///     Deserializes a JSON string to the specified type.
    /// </summary>
    /// <param name="contents">The contents to deserialize.</param>
    /// <param name="options">Options to use when deserializing the file.</param>
    /// <typeparam name="TValue">The expected type.</typeparam>
    /// <returns>The type, or the default for the type.</returns>
    public static TValue? DeserializeString<TValue>(string contents, JsonSerializerOptions? options = null)
    {
        var actualOptions = options ?? JsonConstants.JsonSerializeOptions;

        try
        {
            return JsonSerializer.Deserialize<TValue>(contents, actualOptions);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    ///     Serializes an object to a JSON file.
    /// </summary>
    /// <param name="path">The file to output the JSON string to.</param>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="options">Options to use when serializing the file.</param>
    public static void SerializeToFile(string path, object obj, JsonSerializerOptions? options = null)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        var contents = SerializeToString(obj, options);
        File.WriteAllText(path, contents);
    }

    /// <summary>
    ///     Serializes an object to a JSON string.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="options">Options to use when serializing the file.</param>
    /// <returns>The JSON string representing the object.</returns>
    public static string SerializeToString(object obj, JsonSerializerOptions? options = null)
    {
        var actualOptions = options ?? JsonConstants.JsonSerializeOptions;
        var contents = JsonSerializer.Serialize(obj, actualOptions);

        return contents;
    }
}
