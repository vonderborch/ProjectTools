using System.Diagnostics;
using System.Reflection;

namespace ProjectTools.Core.Constants;

public static class AppConstants
{
    /// <summary>
    ///     The application repository URL
    /// </summary>
    public static string ApplicationRepositoryUrl = "https://github.com/vonderborch/ProjectTools";

    /// <summary>
    ///     The version of the core library
    /// </summary>
    public static string? CoreVersion =
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
}
