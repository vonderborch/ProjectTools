using System.Runtime.InteropServices;
using ProjectTools.Core.Constants;

namespace ProjectTools.Core.Helpers;

/// <summary>
///     Helpers related to the environment the program is running in.
/// </summary>
public static class EnvironmentHelpers
{
    /// <summary>
    ///     Initializes the <see cref="EnvironmentHelpers" /> class.
    /// </summary>
    /// <exception cref="PlatformNotSupportedException">Raised if the OS is not supported.</exception>
    static EnvironmentHelpers()
    {
        Architecture = RuntimeInformation.OSArchitecture;

        var foundOs = false;
        var oses = PythonConstants.PythonInfo.Keys;
        foreach (var os in oses)
        {
            if (RuntimeInformation.IsOSPlatform(os))
            {
                OS = (OSPlatform)os;
                foundOs = true;
                break;
            }
        }

        if (!foundOs)
        {
            throw new PlatformNotSupportedException("The current OS is not supported.");
        }
    }

    /// <summary>
    ///     The current OS the program is running on.
    /// </summary>
    public static OSPlatform OS { get; }

    /// <summary>
    ///     The architecture of the current OS.
    /// </summary>
    public static Architecture Architecture { get; }
}
