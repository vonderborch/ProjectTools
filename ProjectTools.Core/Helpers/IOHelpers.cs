#region

using System.Text;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using ProjectTools.Core.Constants;
using ZipFile = System.IO.Compression.ZipFile;

#endregion

namespace ProjectTools.Core.Helpers;

public static class IOHelpers
{
    /// <summary>
    ///     Archives a directory.
    /// </summary>
    /// <param name="directoryToArchive">The directory to archive.</param>
    /// <param name="archivePath">The archive path.</param>
    /// <param name="skipCleaning">if set to <c>true</c> [skip cleaning].</param>
    public static void ArchiveDirectory(string directoryToArchive, string archivePath, bool skipCleaning,
        int maxRetries = 10, int baseDelayMs = 100, int maxDelayMs = 3000)
    {
        // Delete any existing archive
        if (File.Exists(archivePath))
        {
            File.Delete(archivePath);
        }

        // Create a new archive
        ZipFile.CreateFromDirectory(directoryToArchive, archivePath);

        // Delete the base directory to archive if requested
        if (!skipCleaning)
        {
            for (var attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    Directory.Delete(directoryToArchive, true);
                    return;
                }
                catch (Exception)
                {
                    if (attempt == maxRetries - 1)
                    {
                        throw;
                    }

                    // Exponential backoff: 100ms, 200ms, 400ms, 800ms, 1600ms, etc.
                    var delayMs = baseDelayMs * (int)Math.Pow(2, attempt);
                    delayMs = Math.Min(delayMs, maxDelayMs);
                    Thread.Sleep(delayMs);
                }
            }

            throw new Exception($"Could not delete directory: {directoryToArchive}");
        }
    }

    /// <summary>
    ///     Removes the directory, if allowed.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="forceOverride">Whether we can delete it if it alrady exists.</param>
    /// <returns>True if success, False otherwise.</returns>
    public static bool CleanDirectory(string path, bool forceOverride)
    {
        if (Directory.Exists(path))
        {
            if (!forceOverride)
            {
                return false;
            }

            Directory.Delete(path, true);
        }

        return true;
    }

    /// <summary>
    ///     Removes the file, if allowed.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="forceOverride">Whether we can delete it if it alrady exists.</param>
    /// <returns>True if success, False otherwise.</returns>
    public static bool CleanFile(string path, bool forceOverride)
    {
        if (File.Exists(path))
        {
            if (!forceOverride)
            {
                return false;
            }

            File.Delete(path);
        }

        return true;
    }

    /// <summary>
    ///     Copies the specified directory.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    /// <param name="excludedDirectories">The excluded directories.</param>
    /// <param name="excludedFiles">The excluded files.</param>
    public static void CopyDirectory(string source, string destination, List<string> excludedDirectories,
        List<string> excludedFiles)
    {
        CopyDirectoryHelper(source, source, destination, excludedDirectories, excludedFiles);
    }

    /// <summary>
    ///     Copies the directory helper.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="rootSource">The root source directory.</param>
    /// <param name="destination">The destination.</param>
    /// <param name="excludedDirectories">The excluded directories.</param>
    /// <param name="excludedFiles">The excluded files.</param>
    private static void CopyDirectoryHelper(string source, string rootSource, string destination,
        List<string> excludedDirectories, List<string> excludedFiles)
    {
        CreateDirectoryIfNotExists(destination);

        // Copy over items
        var files = Directory.GetFiles(source)
            .Where(f => Path.GetFileName(f) != TemplateConstants.TemplateSettingsFileName).ToList();
        foreach (var file in files)
        {
            if (!PathHelpers.PathIsInList(file, rootSource, excludedFiles, true))
            {
                var path = Path.Combine(destination, Path.GetFileName(file));

                SafeCopyFile(file, path);
            }
        }

        // Copy over sub-directories
        var directories = Directory.GetDirectories(source);
        foreach (var directory in directories)
        {
            if (!PathHelpers.PathIsInList(directory, rootSource, excludedDirectories, true))
            {
                var dirName = Path.GetFileName(directory);
                var path = Path.Combine(destination, dirName);
                CopyDirectoryHelper(directory, rootSource, path, excludedDirectories, excludedFiles);
            }
        }
    }

    /// <summary>
    ///     Creates the directory if not exists.
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
    ///     Unzips the tar file.
    /// </summary>
    /// <param name="archiveFile">The archive file.</param>
    /// <param name="outputDirectory">The output directory.</param>
    public static void DecompressTarball(Stream archiveFile, string outputDirectory)
    {
        CreateDirectoryIfNotExists(outputDirectory);
        Stream gzipStream = new GZipInputStream(archiveFile);

        var tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
        tarArchive.ExtractContents(outputDirectory);
        tarArchive.Close();

        gzipStream.Close();
    }

    /// <summary>
    ///     Deletes the directory if it exists.
    /// </summary>
    /// <param name="file">The directory.</param>
    public static void DeleteDirectoryIfExists(string file)
    {
        if (Directory.Exists(file))
        {
            Directory.Delete(file, true);
        }
    }

    /// <summary>
    ///     Deletes the file if it exists.
    /// </summary>
    /// <param name="file">The file.</param>
    public static void DeleteFileIfExists(string file)
    {
        if (File.Exists(file))
        {
            File.Delete(file);
        }
    }

    /// <summary>
    ///     Gets the file contents from archive.
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

    public static string GetFileSystemSafeString(string str)
    {
        var safeStr = str;
        safeStr = safeStr.Replace(" ", "_");
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            safeStr = safeStr.Replace(c, '-');
        }

        return safeStr;
    }

    /// <summary>
    ///     Copies a file with retries.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    /// <param name="numTries">The number tries.</param>
    /// <param name="sleepTime">The sleep time.</param>
    private static void SafeCopyFile(string source, string destination, int numTries = 3, int sleepTime = 500)
    {
        if (source == destination)
        {
            return;
        }

        for (var i = 0; i < numTries; i++)
        {
            try
            {
                File.Copy(source, destination, true);
                return;
            }
            catch
            {
                Thread.Sleep(sleepTime);
            }
        }

        throw new Exception($"Could not copy directory '{source}' to destination '{destination}'");
    }

    /// <summary>
    ///     Unzips the directory.
    /// </summary>
    /// <param name="outputDirectory">The output directory.</param>
    /// <param name="archiveFile">The archive file.</param>
    /// <param name="excludedFiles">The excluded files.</param>
    /// <param name="overrideDirectory">if set to <c>true</c> [override directory].</param>
    public static void UnzipDirectory(string outputDirectory, string archiveFile, List<string> excludedFiles,
        bool overrideDirectory = true)
    {
        if (overrideDirectory)
        {
            DeleteDirectoryIfExists(outputDirectory);
        }

        using var file = File.OpenRead(archiveFile);
        using var zip = new ICSharpCode.SharpZipLib.Zip.ZipFile(file);
        foreach (ZipEntry entry in zip)
        {
            var path = Path.Combine(outputDirectory, entry.Name.Replace("/", Path.DirectorySeparatorChar.ToString()));
            var directoryPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (entry.IsDirectory)
                {
                    _ = Directory.CreateDirectory(path);
                }
                else
                {
                    if (excludedFiles.Contains(Path.GetFileName(entry.Name)))
                    {
                        continue;
                    }

                    if (directoryPath is { Length: > 0 })
                    {
                        _ = Directory.CreateDirectory(directoryPath);
                    }

                    var buffer = new byte[4096];
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    using var inputStream = zip.GetInputStream(entry);
                    using var output = File.Create(path);
                    StreamUtils.Copy(inputStream, output, buffer);
                }
            }
        }
    }
}
