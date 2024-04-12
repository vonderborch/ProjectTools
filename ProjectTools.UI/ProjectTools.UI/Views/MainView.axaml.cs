using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.Core;
using ProjectTools.ViewModels;

namespace ProjectTools.Views;

/// <summary>
/// Represents the main view of the application.
/// </summary>
public partial class MainView : UserControl
{
    /// <summary>
    /// Represents the main view of the application.
    /// </summary>
    public MainView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Represents the view model for the main view of the application.
    /// </summary>
    private MainViewModel ViewModel => (MainViewModel)DataContext;

    /// <summary>
    /// Handles the event when a button for changing the page is clicked.
    /// </summary>
    /// <param name="sender">The object that raises the event.</param>
    /// <param name="args">The event arguments.</param>
    public void ChangePageHandler(object sender, RoutedEventArgs args)
    {
        if (sender is Button button)
        {
            var name = button.Name;
            Pages? nextPage = null;

            switch (name)
            {
                case "AboutButton":
                    nextPage = Pages.About;
                    break;
                case "SettingsButton":
                    nextPage = Pages.Settings;
                    break;
                case "PrepareTemplateButton":
                    nextPage = Pages.PrepareTemplate;
                    break;
                case "GenerateProjectButton":
                    nextPage = Pages.GenerateProject;
                    break;
                case "ReportIssueButton":
                    nextPage = Pages.ReportIssue;
                    break;
                case "MakeSuggestionButton":
                    nextPage = Pages.MakeSuggestion;
                    break;
            }

            if (nextPage != null) ViewModel.ChangePage((Pages)nextPage);
        }
    }

    /// <summary>
    /// Handles the event when a button for opening a webpage is clicked.
    /// </summary>
    /// <param name="sender">The object that raises the event.</param>
    /// <param name="args">The event arguments.</param>
    public void OpenWebpageHandler(object sender, RoutedEventArgs args)
    {
        if (sender is Button button)
        {
            var name = button.Name;
            var url = "";

            switch (name)
            {
                case "OpenRepoButton":
                    url = Constants.ApplicationRepositoryUrl;
                    break;
            }

            if (!string.IsNullOrWhiteSpace(url))
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

    /// <summary>
    /// Handles the event when the Exit button is clicked and closes the application.
    /// </summary>
    /// <param name="sender">The object that raises the event.</param>
    /// <param name="args">The event arguments.</param>
    public void ExitAppHandler(object sender, RoutedEventArgs args)
    {
        var parent = (MainWindow?)Parent;
        if (parent != null) parent.Close();
    }
}