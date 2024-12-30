using ProjectTools.Core.Constants;
using ProjectTools.Core.Settings;
using ProjectTools.Core.TemplateRepositories;

namespace ProjectTools.Core;

/// <summary>
///     A class to handle updates to the application.
/// </summary>
public class AppUpdator
{
    /// <summary>
    ///     A method to check for updates to the application.
    /// </summary>
    /// <param name="appName">The name of the application.</param>
    /// <param name="forceCheck">True to force the check, False otherwise.</param>
    /// <returns>True if there is an update, False if there is not.</returns>
    /// <exception cref="Exception">If an issue occurred while checking for application updates.</exception>
    public (string, bool) CheckForUpdates(string appName, bool forceCheck)
    {
        var appSettings = AbstractSettings.LoadOrThrow();
        if (forceCheck || appSettings.ShouldCheckForAppUpdates(appName))
        {
            var client = GitClientManager.ClientManager.GetGitClientForRepo(AppConstants.ApplicationRepositoryUrl);

            if (client == null)
            {
                throw new Exception("No client registered for github.com!");
            }

            var latestReleaseTask = client.Repository.Release.GetLatest(AppConstants.RepoOwner, AppConstants.RepoName);
            latestReleaseTask.Wait();

            var latestRelease = latestReleaseTask.Result;

            if (latestRelease == null)
            {
                throw new Exception("No releases found!");
            }

            appSettings.LastAppUpdateCheck[appName] = DateTime.Now;
            appSettings.Save();
            if (latestRelease.TagName != AppConstants.CoreVersion)
            {
                return (latestRelease.TagName, true);
            }
        }

        return (string.Empty, false);
    }
}
