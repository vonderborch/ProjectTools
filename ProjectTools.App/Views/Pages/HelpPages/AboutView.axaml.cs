using Avalonia.Controls;
using Avalonia.Interactivity;
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

        var doUpdate = await OkDialogBox.OpenDialogBox(this, "Update Check Result", hasUpdateText, 300, 150);

        if (hasUpdate && doUpdate)
        {
            UrlHelpers.OpenUrl(AppConstants.RepoLatestReleaseUrl,
                $"Please go to {AppConstants.RepoLatestReleaseUrl} to download the latest release!");
        }
    }
}
