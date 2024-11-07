using System.IO.Compression;
using ProjectTools.Core.Constants;

namespace ProjectTools.Core.Helpers;

public static class IOHelpers
{
    /// <summary>
    ///     Archives a directory.
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
        ZipFile.CreateFromDirectory(directoryToArchive, archivePath);

        // Delete the base directory to archive if requested
        if (!skipCleaning)
        {
            Thread.Sleep(500);
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    Directory.Delete(directoryToArchive, true);
                    return;
                }
                catch (Exception)
                {
                    // Give some time to let the OS release the directory
                    Thread.Sleep(500);
                }
            }

            throw new Exception($"Could not delete directory: {directoryToArchive}");
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
    public static void CopyDirectory(string source, string destination, List<string> excludedDirectories)
    {
        var actualExcludedDirectories = excludedDirectories.Select(x => x.ToLowerInvariant()).ToList();
        CopyDirectoryHelper(source, destination, actualExcludedDirectories);
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
    ///     Copies the directory helper.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    /// <param name="excludedDirectories">The excluded directories.</param>
    private static void CopyDirectoryHelper(string source, string destination, List<string> excludedDirectories)
    {
        CreateDirectoryIfNotExists(destination);

        // Copy over items
        var files = Directory.GetFiles(source)
            .Where(f => Path.GetFileName(f) != TemplateConstants.TemplateSettingsFileName).ToList();
        for (var i = 0; i < files.Count; i++)
        {
            var path = Path.Combine(destination, Path.GetFileName(files[i]));

            SafeCopyFile(files[i], path);
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

                /*
                var updatedExcludedDirectories = excludedDirectories
                    .Select(x => x.StartsWith(dirName) ? x[dirName.Length..] : x).ToList();
                updatedExcludedDirectories = updatedExcludedDirectories
                    .Where(x => x.Contains(Path.DirectorySeparatorChar)).ToList();
                CopyDirectoryHelper(directories[i], path, updatedExcludedDirectories);
                */
            }
        }
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
}
