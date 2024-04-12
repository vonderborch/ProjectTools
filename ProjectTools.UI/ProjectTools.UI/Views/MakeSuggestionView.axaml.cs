using System;
using System.Diagnostics;
using System.Web;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.Core;

namespace ProjectTools.Views;

/// <summary>
/// Represents a view for making suggestions.
/// </summary>
public partial class MakeSuggestionView : UserControl
{
    /// <summary>
    /// Represents a view for making suggestions.
    /// </summary>
    public MakeSuggestionView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the Create Suggestion button click.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="args">The event arguments.</param>
    public void CreateSuggestionClickHandler(object sender, RoutedEventArgs args)
    {
        var title = TitleTextBox.Text;
        var description = DescriptionTextBox.Text;

        title = $"FEATURE: {title}";
        title = HttpUtility.UrlEncode(title);
        description = HttpUtility.UrlEncode(description);

        var baseUrl = $"{Constants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=enhancement";

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