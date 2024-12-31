using System.Diagnostics;
using System.Reflection;
using System.Text;
using Avalonia.Controls;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.Core.Constants;

namespace ProjectTools.App.Views.Pages;

/// <summary>
///     The about page.
/// </summary>
[PageRegistration("About", Page.About)]
public partial class AboutPage : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AboutPage" /> class.
    /// </summary>
    public AboutPage()
    {
        InitializeComponent();

        var sb = new StringBuilder();
        sb.AppendLine($"Program: {AppConstants.AppNameGui}");
        sb.AppendLine(
            $"Version: {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}");
        sb.AppendLine($"Core Version: {AppConstants.CoreVersion}");
        sb.AppendLine("Author: Christian Webber");
        this.TextBlockAbout.Text = sb.ToString();
    }
}
