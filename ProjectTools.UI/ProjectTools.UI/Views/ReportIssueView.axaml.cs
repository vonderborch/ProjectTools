using System;
using System.Diagnostics;
using System.Web;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.Core;

namespace ProjectTools.Views;

/// <summary>
/// Represents the view for reporting an issue.
/// </summary>
public partial class ReportIssueView : UserControl
{
    /// <summary>
    /// Represents the view for reporting an issue.
    /// </summary>
    public ReportIssueView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event for the "Create Bug Report" button.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="args">The event arguments.</param>
    public void ReportIssueClickHandler(object sender, RoutedEventArgs args)
    {
        var title = TitleTextBox.Text;
        var description = DescriptionTextBox.Text;

        title = $"BUG: {title}";
        title = HttpUtility.UrlEncode(title);
        description = HttpUtility.UrlEncode(description);

        var baseUrl = $"{Constants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=bug";

        try
        {
            var ps = new ProcessStartInfo(url) { UseShellExecute = true, Verb = "open" };
            _ = Process.Start(ps);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}