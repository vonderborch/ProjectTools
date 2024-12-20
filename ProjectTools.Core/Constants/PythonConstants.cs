using System.Runtime.InteropServices;

namespace ProjectTools.Core.Constants;

public static class PythonConstants
{
    public static string PythonVersion = "3.12.8";

    public static Dictionary<OSPlatform, Dictionary<Architecture, Tuple<string, string, string>>> PythonInfo;

    static PythonConstants()
    {
        // Source for Python Builds:
        // https://github.com/indygreg/python-build-standalone
        // Used Release: https://github.com/indygreg/python-build-standalone/releases/tag/20241206
        PythonInfo = new Dictionary<OSPlatform, Dictionary<Architecture, Tuple<string, string, string>>>();

        // Windows
        var windowsInfo = new Dictionary<Architecture, Tuple<string, string, string>>
        {
            {
                Architecture.X64, new Tuple<string, string, string>(
                    "cpython-3.12.8+20241206-x86_64-pc-windows-msvc-shared-install_only",
                    "python.exe",
                    "python3.dll"
                )
            },
            {
                Architecture.X86, new Tuple<string, string, string>(
                    "cpython-3.12.8+20241206-x86_64-pc-windows-msvc-shared-install_only",
                    "python.exe",
                    "python3.dll"
                )
            }
        };
        PythonInfo.Add(OSPlatform.Windows, windowsInfo);

        // Linux
        var linuxInfo = new Dictionary<Architecture, Tuple<string, string, string>>
        {
            {
                Architecture.X64, new Tuple<string, string, string>(
                    "cpython-3.12.8+20241206-x86_64_v2-unknown-linux-gnu-install_only",
                    Path.Combine("bin", "python3.12"),
                    Path.Combine("lib", "libpython3.so")
                )
            },
            {
                Architecture.X86, new Tuple<string, string, string>(
                    "cpython-3.12.8+20241206-x86_64_v2-unknown-linux-gnu-install_only",
                    Path.Combine("bin", "python3.12"),
                    Path.Combine("lib", "libpython3.so")
                )
            }
        };
        PythonInfo.Add(OSPlatform.Linux, linuxInfo);

        // OSX
        var osxInfo = new Dictionary<Architecture, Tuple<string, string, string>>
        {
            {
                Architecture.X64, new Tuple<string, string, string>(
                    "cpython-3.12.8+20241206-x86_64-apple-darwin-install_only",
                    Path.Combine("bin", "python3.12"),
                    Path.Combine("lib", "libpython3.12.dylib")
                )
            },
            {
                Architecture.X86, new Tuple<string, string, string>(
                    "cpython-3.12.8+20241206-x86_64-apple-darwin-install_only",
                    Path.Combine("bin", "python3.12"),
                    Path.Combine("lib", "libpython3.12.dylib")
                )
            },
            {
                Architecture.Arm64, new Tuple<string, string, string>(
                    "cpython-3.12.8+20241206-aarch64-apple-darwin-install_only",
                    Path.Combine("bin", "python3.12"),
                    Path.Combine("lib", "libpython3.12.dylib")
                )
            }
        };
        PythonInfo.Add(OSPlatform.OSX, osxInfo);
    }
}
