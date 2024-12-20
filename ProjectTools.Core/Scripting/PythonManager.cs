using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Settings;

namespace ProjectTools.Core.Scripting;

/// <summary>
///     A manager for Python scripting.
/// </summary>
public sealed class PythonManager
{
    /// <summary>
    ///     The lazy instance of the <see cref="PythonManager" />.
    /// </summary>
    private static readonly Lazy<PythonManager> Lazy = new(() => new PythonManager());

    /// <summary>
    ///     Initializes a new instance of the <see cref="PythonManager" /> class.
    /// </summary>
    private PythonManager()
    {
        var pythonInfo = DetermineAndExtractPythonVersion();

        this.PythonExecutable = Path.Combine(this.PythonDirectory, pythonInfo.Item2);
    }

    /// <summary>
    ///     Gets the instance of the <see cref="PythonManager" />.
    /// </summary>
    public static PythonManager Manager => Lazy.Value;

    /// <summary>
    ///     The directory where Python is stored.
    /// </summary>
    public string PythonDirectory => Path.Combine(PathConstants.CoreDirectory, "python");

    /// <summary>
    ///     The executable for Python.
    /// </summary>
    public string PythonExecutable { get; }

    /// <summary>
    ///     Determines the Python version and extracts it.
    /// </summary>
    /// <returns>Information on the Python version.</returns>
    private Tuple<string, string> DetermineAndExtractPythonVersion()
    {
        var architecture = EnvironmentHelpers.Architecture;
        var platform = EnvironmentHelpers.OS;

        var pythonInfo = PythonConstants.PythonInfo[platform][architecture];
        if (Directory.Exists(this.PythonDirectory) &&
            AbstractSettings.LoadOrThrow(false).PythonVersion != PythonConstants.PythonVersion)
        {
            Directory.Delete(this.PythonDirectory, true);
        }

        if (!Directory.Exists(this.PythonDirectory))
        {
            var fileStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"ProjectTools.Core.Scripting.{pythonInfo.Item1}.tar.gz");
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
