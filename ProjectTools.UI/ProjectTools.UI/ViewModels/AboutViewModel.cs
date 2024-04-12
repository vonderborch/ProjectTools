using System.Diagnostics;
using System.Reflection;
using ProjectTools.Core;

namespace ProjectTools.ViewModels;

/// <summary>
/// Represents the view model for the AboutView.
/// </summary>
public class AboutViewModel : ViewModelBase
{
    /// <summary>
    /// Assembly information for the program.
    /// </summary>
    public FileVersionInfo AssemblyInfo => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

    /// <summary>
    /// The name of the program.
    /// </summary>
    public string ProgramName => "Project Tools";

    /// <summary>
    /// Version information for the program.
    /// </summary>
    public string VersionText => $"Program Version: v{ProgramVersion} (Core v{CoreVersionNumber})";

    /// <summary>
    /// Developer information for the program.
    /// </summary>
    public string DeveloperText => $"Developed by {Developer} ({CopyrightInfo})";

    /// <summary>
    /// Program version number.
    /// </summary>
    public string ProgramVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";

    /// <summary>
    /// Core version number.
    /// </summary>
    public string CoreVersionNumber => Constants.Version;

    /// <summary>
    /// Developer name.
    /// </summary>
    public string Developer => AssemblyInfo.CompanyName ?? "UNKNOWN";

    /// <summary>
    /// Copyright information.
    /// </summary>
    public string CopyrightInfo => "© 2024";
}