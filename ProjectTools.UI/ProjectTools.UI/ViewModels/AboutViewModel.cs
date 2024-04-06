using System.Diagnostics;
using System.Reflection;
using ProjectTools.Core;

namespace ProjectTools.ViewModels;

public class AboutViewModel : ViewModelBase
{
    public FileVersionInfo AssemblyInfo => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
    public string ProgramName => "Project Tools";
    public string VersionText => $"Program Version: v{ProgramVersion} (Core v{CoreVersionNumber})";
    public string DeveloperText => $"Developed by {Developer} ({CopyrightInfo})";
    
    public string ProgramVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
    public string CoreVersionNumber => Constants.Version;
    public string Developer => AssemblyInfo.CompanyName ?? "UNKNOWN";
    public string CopyrightInfo => "© 2024";
}