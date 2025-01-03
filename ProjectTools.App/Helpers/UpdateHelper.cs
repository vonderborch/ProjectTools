#region

using Avalonia.Controls;
using ProjectTools.App.Dialogs.YesNoDialogBox;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

#endregion

namespace ProjectTools.App.Helpers;

/// <summary>
///     Various helper methods for updating the application.
/// </summary>
public static class UpdateHelper
{
    /// <summary>
    ///     Check for updates to the application.
    /// </summary>
    /// <param name="parentControl">The parent control.</param>
    /// <param name="force">Whether to force the check or not.</param>
    public static async void CheckForUpdates(UserControl parentControl, bool force)
    {
        AppUpdator updator = new();
        var (newVersion, hasUpdate) = updator.CheckForUpdates(AppConstants.AppNameGui, force);

        var hasUpdateText = hasUpdate ? $"There is an update available (v{newVersion})!" : "You are up to date!";

        var doUpdate = await YesNoDialogBox.Open(
            parentControl,
            "Update Check Result",
            hasUpdateText,
            300,
            150,
            yesButtonText: "OK",
            showNoButton: false
        );

        if (hasUpdate && doUpdate)
        {
            UrlHelpers.OpenUrl(AppConstants.RepoLatestReleaseUrl,
                $"Please go to {AppConstants.RepoLatestReleaseUrl} to download the latest release!");
        }
    }
}
