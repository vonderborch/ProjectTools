using System.Web;
using CommandLine;
using Microsoft.VisualBasic;
using ProjectTools.CL.Helpers;
using ProjectTools.CL.Options;
using ProjectTools.Core.Constants;

/// <summary>
/// A command to make a suggestion for the program with.
/// </summary>
[Verb("suggestion", HelpText = "Make a suggestion for the program")]
public class MakeSuggestion : AbstractOption
{
    /// <summary>
    /// Gets or sets the description of the suggestion.
    /// </summary>
    /// <value>The description.</value>
    [Option('d', "description", Required = false, HelpText = "The description of the new feature or functionality")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title of the suggestion.
    /// </summary>
    /// <value>The title.</value>
    [Option('t', "title", Required = false, HelpText = "The title of the new feature or functionality")]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        var title = Title;
        var description = Description;

        if (Silent && (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description)))
        {
            throw new Exception("Error: Title and Description must be specified when running in silent mode!");
        }
        else if (!Silent)
        {
            // get issue title and description
            title = ConsoleHelpers.GetInput("Suggestion Title", title);
            description = ConsoleHelpers.GetInput("Suggestion Description", description);

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                throw new Exception("Error: Title and Description must be specified!");
            }
        }

        // Construct Urls
        title = $"FEATURE: {title}";
        title = HttpUtility.UrlEncode(title);
        description = HttpUtility.UrlEncode(description);

        var baseUrl = $"{AppConstants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=enhancement";
        // Report issue
        LogMessage("Opening browser to make suggestion ...");
        UrlHelpers.OpenUrl(url, $"Please go to {baseUrl} to make a suggestion!");

        return string.Empty;
    }

    /// <summary>
    /// Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (MakeSuggestion)option;
        Description = options.Description;
        Title = options.Title;
        Silent = options.Silent;
    }
}