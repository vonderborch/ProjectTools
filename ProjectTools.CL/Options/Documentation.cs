using CommandLine;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to report an issue with the program.
/// </summary>
[Verb("documentation", HelpText = "Go to the wiki for documentation on the program.")]
[MenuMetadata(1)]
public class Documentation : AbstractOption
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
        UrlHelpers.OpenUrl(url, $"Please go to {url} to open the wiki!");

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
