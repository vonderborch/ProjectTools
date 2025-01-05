using CommandLine;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to check for application updates.
/// </summary>
[Verb("update", HelpText = "Checks for updates to the program.")]
[MenuMetadata(10, "-f")]
public class CheckForUpdates : AbstractOption
{
    /// <summary>
    ///     A flag indicating whether to force-check for application updates.
    /// </summary>
    [Option('f', "force-check", Required = false, Default = false,
        HelpText = "If flag is provided, the program will force-check for application updates.")]
    public bool ForceCheck { get; set; }

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        AppUpdator updator = new();
        var (newVersion, hasUpdate) = updator.CheckForUpdates(AppConstants.AppNameCommandLine, this.ForceCheck);

        if (hasUpdate)
        {
            LogMessage($"An update is available (v{newVersion}), opening browser to latest release...");
            UrlHelpers.OpenUrl(AppConstants.RepoLatestReleaseUrl,
                $"Please go to {AppConstants.RepoLatestReleaseUrl} to download the latest release!");
        }

        return string.Empty;
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (UpdateTemplates)option;
        this.ForceCheck = options.ForceCheck;
    }
}
