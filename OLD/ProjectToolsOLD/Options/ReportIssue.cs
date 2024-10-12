using System.Web;
using CommandLine;
using ProjectToolsOLD.CoreOLD;
using ProjectToolsOLD.Helpers;

namespace ProjectToolsOLD.Options
{
    /// <summary>
    /// A command to report an issue with the program
    /// </summary>
    /// <seealso cref="AbstractOption"/>
    [Verb("report-issue", HelpText = "Report an issue with the program")]
    internal class ReportIssue : AbstractOption
    {
        /// <summary>
        /// Gets or sets the description of the issue.
        /// </summary>
        /// <value>The description.</value>
        [Option('d', "description", Required = false, HelpText = "The description of the issue")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the issue.
        /// </summary>
        /// <value>The title.</value>
        [Option('t', "title", Required = false, HelpText = "The title of the issue")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Executes the report issue steps with the specified options.
        /// </summary>
        /// <returns>The result of the execution.</returns>
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

            var baseUrl = $"{Constants.ApplicationRepositoryUrl}/issues/new";
            var url = $"{baseUrl}?title={title}&body={description}&labels=bug";

            // Report issue
            LogMessage("Opening browser to report issue ...");
            UrlHelpers.OpenUrl(url, $"Please go to {baseUrl} to file a bug!");

            return string.Empty;
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        /// <param name="option">The option.</param>
        protected override void SetOptions(AbstractOption option)
        {
            var options = (ReportIssue)option;
            Description = options.Description;
            Title = options.Title;
            Silent = options.Silent;
        }
    }
}
