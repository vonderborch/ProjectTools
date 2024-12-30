using System.Web;
using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core.Constants;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to report an issue with the program.
/// </summary>
[Verb("report-issue", HelpText = "Report an issue with the program.")]
[MenuMetadata(2)]
public class ReportIssue : SilenceAbleAbstractOption
{
    /// <summary>
    ///     Gets or sets the description of the issue.
    /// </summary>
    /// <value>The description.</value>
    [Option('d', "description", Required = false, HelpText = "The description of the issue.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the title of the issue.
    /// </summary>
    /// <value>The title.</value>
    [Option('t', "title", Required = false, HelpText = "The title of the issue.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        var title = this.Title;
        var description = this.Description;

        if (this.Silent && (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description)))
        {
            throw new Exception("Error: Title and Description must be specified when running in silent mode!");
        }

        if (!this.Silent)
        {
            // get issue title and description
            title = ConsoleHelpers.GetInput("Issue Title", title);
            description = ConsoleHelpers.GetInput("Issue Description", description);

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                throw new Exception("Error: Title and Description must be specified!");
            }
        }

        // Construct Urls
        title = $"BUG: {title}";
        title = HttpUtility.UrlEncode(title);
        description = HttpUtility.UrlEncode(description);

        var baseUrl = $"{AppConstants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=bug";

        // Report issue
        LogMessage("Opening browser to report issue ...");
        UrlHelpers.OpenUrl(url, $"Please go to {baseUrl} to file a bug!");

        return string.Empty;
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (ReportIssue)option;
        this.Description = options.Description;
        this.Title = options.Title;
        this.Silent = options.Silent;
    }
}
