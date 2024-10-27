using System.Diagnostics;
using System.Reflection;

namespace ProjectTools.Core.Constants;

public static class AppConstants
{
    public static string? CoreVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
}
