using System.Text.Json;

namespace ProjectTools.Core.Helpers
{
    /// <summary>
    /// Helpers for dealing with JSON objects
    /// </summary>
    public static class JsonHelpers
    {
        /// <summary>
        /// Deserializes a file in an archive.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="file">The file.</param>
        /// <returns>The deserialized object</returns>
        public static T? DeserializeArchivedFile<T>(string archive, string file) where T : class
        {
            var contents = IOHelpers.GetFileContentsFromArchivedFile(archive, file);
            return DeserializeContents<T>(contents);
        }

        /// <summary>
        /// Deserializes the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The deserialized object</returns>
        public static T? DeserializeFile<T>(string file) where T : class
        {
            var contents = File.ReadAllText(file);
            return DeserializeContents<T>(contents);
        }

        /// <summary>
        /// Writes the object to file.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        public static void WriteObjectToFile<T>(T obj, string file) where T : class
        {
            var contents = JsonSerializer.Serialize(obj, Constants.JsonSerializeOptions);
            File.WriteAllText(file, contents);
        }

        /// <summary>
        /// Deserializes the contents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contents">The contents.</param>
        /// <returns>The deserialized object, or null if deserialization fails.</returns>
        /// <exception cref="System.IO.InvalidDataException">
        /// Failed to deserialize contents: {contents}. Inner exception: {Environment.NewLine}{ex.Message}
        /// </exception>
        private static T? DeserializeContents<T>(string contents) where T : class
        {
            if (string.IsNullOrWhiteSpace(contents))
            {
                return null;
            }

            try
            {
                var output = JsonSerializer.Deserialize<T>(contents, Constants.JsonSerializeOptions);
                return output;
            }
            catch
            {
                throw new InvalidDataException($"Failed to deserialize contents: {contents}");
            }
        }
    }
}
