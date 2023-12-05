using System.Text;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ProjectTools.Core.Helpers
{
    /// <summary>
    /// Helpers when dealing with archive files
    /// </summary>
    public static class ArchiveHelpers
    {
        /// <summary>
        /// Gets the file contents from archive.
        /// </summary>
        /// <param name="archivePath">The archive path.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>The contents of the file, if found</returns>
        public static string GetFileContentsFromArchive(string archivePath, string file, int bufferSize = 4096)
        {
            var contents = string.Empty;
            using var fileStream = File.OpenRead(archivePath);
            using var zip = new ZipFile(fileStream);
            foreach (ZipEntry entry in zip)
            {
                if (Path.GetFileName(entry.Name) == file)
                {
                    contents = string.Empty;
                    using var inputStream = zip.GetInputStream(entry);
                    using var output = new MemoryStream();
                    var buffer = new byte[bufferSize];
                    StreamUtils.Copy(inputStream, output, buffer);
                    contents = Encoding.UTF8.GetString(output.ToArray());

                    break;
                }
            }

            return contents;
        }
    }
}
