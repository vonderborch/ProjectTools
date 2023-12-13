using System.Text;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ProjectTools.Core.Helpers
{
    internal class IOHelpers
    {
        /// <summary>
        /// Archives a directory.
        /// </summary>
        /// <param name="directoryToArchive">The directory to archive.</param>
        /// <param name="archivePath">The archive path.</param>
        /// <param name="skipCleaning">if set to <c>true</c> [skip cleaning].</param>
        public static void ArchiveDirectory(string directoryToArchive, string archivePath, bool skipCleaning)
        {
            // Delete any existing archive
            if (File.Exists(archivePath))
            {
                File.Delete(archivePath);
            }

            // Create a new archive
            System.IO.Compression.ZipFile.CreateFromDirectory(directoryToArchive, archivePath);

            // Delete the base directory to archive if requested
            if (!skipCleaning)
            {
                Thread.Sleep(500);
                for (var i = 0; i < 10; i++)
                {
                    try
                    {
                        Directory.Delete(directoryToArchive, true);
                        break;
                    }
                    catch (Exception)
                    {
                        // Give some time to let the OS release the directory
                        Thread.Sleep(500);
                    }
                }
            }
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="excludedDirectories">The excluded directories.</param>
        public static void CopyDirectory(string source, string destination, List<string> excludedDirectories)
        {
            DeleteDirectoryIfExists(destination, true);

            var actualExcludedDirectories = excludedDirectories.Select(x => x.ToLowerInvariant()).ToList();
            CopyDirectoryHelper(source, destination, actualExcludedDirectories);
        }

        /// <summary>
        /// Creates the directory if not exists.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void CreateDirectoryIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                _ = Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Creates the file if not exists.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="defaultContents">The default contents. Defaults to an empty string</param>
        public static void CreateFileIfNotExists(string file, string defaultContents = "")
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, defaultContents);
            }
        }

        /// <summary>
        /// Deletes the directory if it exists.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="createDirectory">if set to <c>true</c> [create directory].</param>
        public static void DeleteDirectoryIfExists(string directory, bool createDirectory = false)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            if (createDirectory)
            {
                CreateDirectoryIfNotExists(directory);
            }
        }

        /// <summary>
        /// Deletes the file if it exists.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="createFile">if set to <c>true</c> [create file].</param>
        public static void DeleteFileIfExists(string file, bool createFile = false)
        {
            if (Directory.Exists(file))
            {
                Directory.Delete(file, true);
            }

            if (createFile)
            {
                CreateFileIfNotExists(file);
            }
        }

        /// <summary>
        /// Gets the file contents from archive.
        /// </summary>
        /// <param name="archivePath">The archive path.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>The contents of the file, if found</returns>
        public static string GetFileContentsFromArchivedFile(string archivePath, string file, int bufferSize = 4096)
        {
            var contents = string.Empty;

            using (var fileStream = File.OpenRead(archivePath))
            {
                using var zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(fileStream);
                foreach (ZipEntry entry in zipFile)
                {
                    if (Path.GetFileName(entry.Name) == file)
                    {
                        contents = string.Empty;
                        using var inputStream = zipFile.GetInputStream(entry);
                        using var output = new MemoryStream();
                        var buffer = new byte[bufferSize];
                        StreamUtils.Copy(inputStream, output, buffer);
                        contents = Encoding.UTF8.GetString(output.ToArray());

                        break;
                    }
                }
            }

            return contents;
        }

        /// <summary>
        /// Copies the directory helper.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="excludedDirectories">The excluded directories.</param>
        private static void CopyDirectoryHelper(string source, string destination, List<string> excludedDirectories)
        {
            IOHelpers.CreateDirectoryIfNotExists(destination);

            // Copy over items
            var files = Directory.GetFiles(source);
            for (var i = 0; i < files.Length; i++)
            {
                var path = Path.Combine(destination, Path.GetFileName(files[i]));

                CopySafe(files[i], path);
            }

            // Copy over sub-directories
            var directories = Directory.GetDirectories(source);
            for (var i = 0; i < directories.Length; i++)
            {
                var dirName = Path.GetFileName(directories[i]);
                var dirNameLower = dirName.ToLowerInvariant();
                if (!excludedDirectories.Contains(dirNameLower))
                {
                    var path = Path.Combine(destination, dirName);
                    CopyDirectoryHelper(directories[i], path, excludedDirectories);

                    var nameWithSeperator = $"{dirName}\\";
                    var updatedExcludedDirectories = excludedDirectories.Select(x => x.StartsWith(dirName) ? x[dirName.Length..] : x).ToList();
                    updatedExcludedDirectories = updatedExcludedDirectories.Where(x => x.Contains(Path.DirectorySeparatorChar)).ToList();
                    CopyDirectoryHelper(directories[i], path, excludedDirectories);
                }
            }
        }

        /// <summary>
        /// Copies a file with retries.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="numTries">The number tries.</param>
        /// <param name="sleepTime">The sleep time.</param>
        private static void CopySafe(string source, string destination, int numTries = 3, int sleepTime = 500)
        {
            for (var i = 0; i < numTries; i++)
            {
                try
                {
                    if (File.Exists(destination))
                    {
                        File.Delete(destination);
                    }

                    File.Copy(source, destination, true);
                }
                catch
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }
    }
}
