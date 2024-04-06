using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.Core;
using ProjectTools.ViewModels;

namespace ProjectTools.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
    
    private MainViewModel ViewModel => (MainViewModel)DataContext;
    
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

            if (nextPage != null)
            {
                ViewModel.ChangePage((Pages)nextPage);
            }
        }
    }
    
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
            {
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
    }
    
    public void ExitAppHandler(object sender, RoutedEventArgs args)
    {
        var parent = (MainWindow?)Parent;
        if (parent != null)
        {
            parent.Close();
        }
    }
}