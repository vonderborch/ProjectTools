#region

using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Settings;

#endregion

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
                proc.WaitForExit();
            }
        }

        return pythonInfo;
    }

    /// <summary>
    ///     Executes a Python script.
    /// </summary>
    /// <param name="executionDirectory">The execution directory.</param>
    /// <param name="scriptPath">The path to the script in the execution directory.</param>
    /// <param name="outputLogger">The output logger.</param>
    /// <param name="baseIndent">The base indentation to output logs.</param>
    /// <returns>boolean (True = success, False = failure) and optionally an exception.</returns>
    public (bool, Exception?) ExecuteScript(string executionDirectory, string scriptPath, Logger outputLogger,
        int baseIndent)
    {
        var script = Path.Combine(executionDirectory, scriptPath);
        try
        {
            var startInfo = GetProcessStartInfo(Manager.PythonExecutable, $"{script}", executionDirectory);
            using Process proc = new();
            proc.StartInfo = startInfo;
            proc.Start();
            proc.WaitForExit();

            using var outputStream = proc.StandardOutput;
            outputLogger.Log("Output Stream:", baseIndent + 2);
            var line = outputStream.ReadLine();
            while (line != null)
            {
                outputLogger.Log(line, baseIndent + 4);
                line = outputStream.ReadLine();
            }

            using var errorStream = proc.StandardError;
            outputLogger.Log("Output Error Stream:", baseIndent + 2);
            line = errorStream.ReadLine();
            while (line != null)
            {
                outputLogger.Log(line, baseIndent + 4);
                line = errorStream.ReadLine();
            }

            return (true, null);
        }
        catch (Exception e)
        {
            return (false, e);
        }
    }

    /// <summary>
    ///     Gets the process start info.
    /// </summary>
    /// <param name="fileName">The executable's file name.</param>
    /// <param name="script">The script to execute.</param>
    /// <param name="workingDirectory">The working directory.</param>
    /// <returns>The start info.</returns>
    private ProcessStartInfo GetProcessStartInfo(string fileName, string script, string workingDirectory)
    {
        if (EnvironmentHelpers.OS == OSPlatform.Windows)
        {
            return new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = $"\"{script}\"",
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }

        return new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = $"\"{script}\"",
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
    }
}
