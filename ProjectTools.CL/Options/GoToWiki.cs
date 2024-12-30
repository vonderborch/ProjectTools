using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core.Constants;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to report an issue with the program.
/// </summary>
[Verb("wiki", HelpText = "Go to the wiki for instructions on the program.")]
[MenuMetadata(1)]
public class GoToWiki : AbstractOption
{
    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        var url = $"{AppConstants.ApplicationRepositoryUrl}/wiki";

        // Report issue
        LogMessage("Opening browser to the wiki...");
        UrlHelpers.OpenUrl(url, $"Please go to {url} to file a bug!");

        return string.Empty;
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
    }
}
