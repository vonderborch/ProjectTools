using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Settings;

namespace ProjectTools.Core.Scripting;

public sealed class PythonManager
{
    private static readonly Lazy<PythonManager> Lazy = new(() => new PythonManager());

    private PythonManager()
    {
        var pythonInfo = DetermineAndExtractPythonVersion();

        this.PythonExecutable = Path.Combine(this.PythonDirectory, pythonInfo.Item2);
        this.PythonDll = Path.Combine(this.PythonDirectory, pythonInfo.Item3);
    }

    public static PythonManager Manager => Lazy.Value;

    public string PythonDirectory => Path.Combine(PathConstants.CoreDirectory, "python");

    public string PythonDll { get; }

    public string PythonExecutable { get; }

    private Tuple<string, string, string> DetermineAndExtractPythonVersion()
    {
        var architecture = RuntimeInformation.OSArchitecture;
        OSPlatform platform;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            platform = OSPlatform.Windows;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            platform = OSPlatform.Linux;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            platform = OSPlatform.OSX;
        }
        else
        {
            throw new PlatformNotSupportedException("The current platform is not supported.");
        }

        var pythonInfo = PythonConstants.PythonInfo[platform][architecture];
        if (Directory.Exists(this.PythonDirectory) &&
            AbstractSettings.LoadOrThrow(false).PythonVersion != PythonConstants.PythonVersion)
        {
            Directory.Delete(this.PythonDirectory, true);
        }

        if (!Directory.Exists(this.PythonDirectory))
        {
            var fileStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"ProjectTools.Core.PythonVersions.{pythonInfo.Item1}.tar.gz");
            IOHelpers.DecompressTarball(fileStream, PathConstants.CoreDirectory);
            fileStream.Close();

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = "chmod",
                    Arguments = $""""755 {Path.Combine(this.PythonDirectory, pythonInfo.Item2)}"""",
                    UseShellExecute = true
                };
                Process proc = new()
                {
                    StartInfo = startInfo
                };
                proc.Start();
            }
        }

        return pythonInfo;
    }
}
