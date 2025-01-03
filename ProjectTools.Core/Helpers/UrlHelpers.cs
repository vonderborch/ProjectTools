using System.Diagnostics;

namespace ProjectTools.Core.Helpers;

/// <summary>
///     Various helpers for dealing with URLs.
/// </summary>
public class UrlHelpers
{
    /// <summary>
    ///     Opens the URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="errorMessage">The error message.</param>
    public static void OpenUrl(string url, string errorMessage)
    {
        try
        {
            var ps = new ProcessStartInfo(url) { UseShellExecute = true, Verb = "open" };
            _ = Process.Start(ps);
        }
        catch
        {
            Console.WriteLine(errorMessage);
        }
    }
}
