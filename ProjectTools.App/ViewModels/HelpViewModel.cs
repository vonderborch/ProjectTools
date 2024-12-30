using System.Diagnostics;
using System.Reflection;
using System.Text;
using ProjectTools.Core.Constants;
using ReactiveUI;

namespace ProjectTools.App.ViewModels;

public class HelpViewModel : ReactiveObject
{
    public HelpViewModel()
    {
        this.AppName = AppConstants.AppNameGui;
        this.AppVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        this.CoreVersion = AppConstants.CoreVersion;
        this.Author = "Christian Webber";

        var sb = new StringBuilder();
        sb.AppendLine($"Program: {this.AppName}");
        sb.AppendLine($"Version: {this.AppVersion}");
        sb.AppendLine($"Core Version: {this.CoreVersion}");
        sb.AppendLine($"Author: {this.Author}");
        this.AboutText = sb.ToString();
    }

    public string AppName { get; }

    public string AppVersion { get; }

    public string CoreVersion { get; }

    public string Author { get; }

    public string AboutText { get; }
}
