namespace ProjectTools.Core.Helpers;

public static class PathHelpers
{
    /// <summary>
    ///     Checks if the given path relative to a root path is in a list of paths.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="paths">The paths to check.</param>
    /// <param name="checkWithRegex">If set to <c>true</c> check with regex.</param>
    /// <returns>True if contained, False otherwise.</returns>
    public static bool PathIsInList(string path, string rootPath, List<string> paths, bool checkWithRegex = false)
    {
        var relativePath = Path.GetRelativePath(rootPath, path);

        foreach (var pathToCheck in paths)
        {
            if (relativePath == pathToCheck)
            {
                return true;
            }

            if (pathToCheck.Contains("*"))
            {
                var basePathToCheck = pathToCheck.Replace("*", "");
                if (pathToCheck.StartsWith("*") && Path.GetFileName(relativePath) == basePathToCheck)
                {
                    return true;
                }

                if (pathToCheck.EndsWith("*") && relativePath.StartsWith(basePathToCheck))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
