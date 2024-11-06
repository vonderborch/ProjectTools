namespace ProjectTools.Core.Helpers;

public static class PathHelpers
{
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
            if (!forceOverride) return false;

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
            if (!forceOverride) return false;

            File.Delete(path);
        }

        return true;
    }
}
