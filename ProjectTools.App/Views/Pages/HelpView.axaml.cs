using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;
using ProjectTools.App.Views.Pages.HelpPages;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.App.Views.Pages;

public partial class HelpView : UserControl
{
    private readonly HelpViewModel _subContext;

    public HelpView()
    {
        InitializeComponent();

        this._subContext = new HelpViewModel();
        ChangeSubView(SubViews.About);
    }

    public void ButtonAbout_Click(object sender, RoutedEventArgs args)
    {
        ChangeSubView(SubViews.About);
    }

    public void ButtonDocumentation_Click(object sender, RoutedEventArgs args)
    {
        var url = $"{AppConstants.ApplicationRepositoryUrl}/wiki";
        UrlHelpers.OpenUrl(url, $"Please go to {url} to go to the wiki!");
    }

    public void ButtonReportIssue_Click(object sender, RoutedEventArgs args)
    {
        ChangeSubView(SubViews.ReportIssue);
    }

    public void ButtonMakeSuggestion_Click(object sender, RoutedEventArgs args)
    {
        ChangeSubView(SubViews.MakeSuggestion);
    }

    private void ChangeSubView(SubViews newView)
    {
        UserControl newControl;

        switch (newView)
        {
            case SubViews.About:
                newControl = new AboutView();
                break;
            case SubViews.ReportIssue:
                newControl = new ReportIssue();
                break;
            case SubViews.MakeSuggestion:
                newControl = new MakeSuggestion();
                break;
            default:
                throw new Exception("Unknown view!");
        }

        newControl.DataContext = this._subContext;
        this.SubViewer.Content = newControl;
    }

    private enum SubViews
    {
        About,
        ReportIssue,
        MakeSuggestion
    }
}
