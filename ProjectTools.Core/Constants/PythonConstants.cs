using System.Runtime.InteropServices;

namespace ProjectTools.Core.Constants;

/// <summary>
///     Constants related to Python.
/// </summary>
public static class PythonConstants
{
    /// <summary>
    ///     The current version of Python being used.
    /// </summary>
    public static string PythonVersion = "3.12.8";

    /// <summary>
    ///     Information on Python builds for different platforms and architectures.
    ///     Key: OS Platform
    ///     Value: Dictionary of Architecture and Tuple of Python Archive Name and Python Executable Name/Path
    /// </summary>
    public static Dictionary<OSPlatform, Dictionary<Architecture, Tuple<string, string>>> PythonInfo;

    /// <summary>
    ///     Initializes the <see cref="PythonConstants" /> class.
    /// </summary>
    static PythonConstants()
    {
        // Source for Python Builds:
        // https://github.com/indygreg/python-build-standalone
        // Used Release: https://github.com/indygreg/python-build-standalone/releases/tag/20241206
        PythonInfo = new Dictionary<OSPlatform, Dictionary<Architecture, Tuple<string, string>>>();

        // Windows
        var windowsInfo = new Dictionary<Architecture, Tuple<string, string>>
        {
            {
                Architecture.X64, new Tuple<string, string>(
                    "cpython-3.12.8+20241206-x86_64-pc-windows-msvc-shared-install_only",
                    "python.exe"
                )
            },
            {
                Architecture.X86, new Tuple<string, string>(
                    "cpython-3.12.8+20241206-x86_64-pc-windows-msvc-shared-install_only",
                    "python.exe"
                )
            }
        };
        PythonInfo.Add(OSPlatform.Windows, windowsInfo);

        // Linux
        var linuxInfo = new Dictionary<Architecture, Tuple<string, string>>
        {
            {
                Architecture.X64, new Tuple<string, string>(
                    "cpython-3.12.8+20241206-x86_64_v2-unknown-linux-gnu-install_only",
                    Path.Combine("bin", "python3.12")
                )
            },
            {
                Architecture.X86, new Tuple<string, string>(
                    "cpython-3.12.8+20241206-x86_64_v2-unknown-linux-gnu-install_only",
                    Path.Combine("bin", "python3.12")
                )
            }
        };
        PythonInfo.Add(OSPlatform.Linux, linuxInfo);

        // OSX
        var osxInfo = new Dictionary<Architecture, Tuple<string, string>>
        {
            {
                Architecture.X64, new Tuple<string, string>(
                    "cpython-3.12.8+20241206-x86_64-apple-darwin-install_only",
                    Path.Combine("bin", "python3.12")
                )
            },
            {
                Architecture.X86, new Tuple<string, string>(
                    "cpython-3.12.8+20241206-x86_64-apple-darwin-install_only",
                    Path.Combine("bin", "python3.12")
                )
            },
            {
                Architecture.Arm64, new Tuple<string, string>(
                    "cpython-3.12.8+20241206-aarch64-apple-darwin-install_only",
                    Path.Combine("bin", "python3.12")
                )
            }
        };
        PythonInfo.Add(OSPlatform.OSX, osxInfo);
    }
}
