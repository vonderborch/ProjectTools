using System.Diagnostics;
using System.Reflection;

namespace ProjectTools.Core.Constants;

public static class AppConstants
{
    /// <summary>
    ///     The name of the application
    /// </summary>
    public static string AppName = "ProjectTools";

    /// <summary>
    ///     The application repository URL
    /// </summary>
    public static string ApplicationRepositoryUrl = "https://github.com/vonderborch/ProjectTools";

    /// <summary>
    ///     The owner of the repository.
    /// </summary>
    public static string RepoOwner = "vonderborch";

    /// <summary>
    ///     The name of the repository.
    /// </summary>
    public static string RepoName = "ProjectTools";

    /// <summary>
    ///     The URL to the latest release of the repository.
    /// </summary>
    public static string RepoLatestReleaseUrl = "https://github.com/vonderborch/ProjectTools/releases/latest";

    /// <summary>
    ///     The version of the core library
    /// </summary>
    public static string? CoreVersion =
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

    /// <summary>
    ///     The name of the Command-Line app.
    /// </summary>
    public static string AppNameCommandLine = "ProjectTools.CL";

    /// <summary>
    ///     The name of the GUI app.
    /// </summary>
    public static string AppNameGui = "ProjectTools.App";
}
