using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.App.Views.Pages.HelpPages;

public partial class AboutView : UserControl
{
    public AboutView()
    {
        InitializeComponent();
    }

    public async void ButtonUpdateCheck_Click(object sender, RoutedEventArgs args)
    {
        AppUpdator updator = new();
        var (newVersion, hasUpdate) = updator.CheckForUpdates(AppConstants.AppNameGui, true);
        
        var hasUpdateText = hasUpdate ? $"There is an update available (v{newVersion})!" : "You are up to date!";
        var okDialogBoxViewModel = new OkDialogBoxViewModel("Update Check Result", hasUpdateText);
        var okDialogBox = new OkDialogBox
        {
            DataContext = okDialogBoxViewModel,
            Width = 300,
            Height = 150
        };

        await okDialogBox.ShowDialog(TopLevel.GetTopLevel(this) as Window);
        if (hasUpdate && okDialogBoxViewModel.ResultIsOk.HasValue && okDialogBoxViewModel.ResultIsOk.Value)
        {
            UrlHelpers.OpenUrl(AppConstants.RepoLatestReleaseUrl,
                $"Please go to {AppConstants.RepoLatestReleaseUrl} to download the latest release!");
        }
    }
}
