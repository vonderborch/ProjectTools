using CommandLine;
using ProjectTools.Core;
using ProjectTools.Core.Templating.Repositories;
using ProjectTools.Helpers;

namespace ProjectTools.Options
{
    [Verb("update-templates", HelpText = "Checks for updated templates")]
    internal class UpdateTemplates : AbstractOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTemplates"/> class.
        /// </summary>
        public UpdateTemplates()
        {
            AllowTemplateUpdates = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [force update].
        /// </summary>
        /// <value><c>true</c> if [force update]; otherwise, <c>false</c>.</value>
        [Option('f', "force", Required = false, Default = false, HelpText = "If flag is provided, the program will force all templates to redownload.")]
        public bool ForceUpdate { get; set; }

        /// <summary>
        /// Executes what this option represents.
        /// </summary>
        /// <returns>The result of the execution.</returns>
        public override string Execute()
        {
            _ = LogMessage("Checking for new or updated templates...");
            var startTime = DateTime.Now;
            (var totalTemplates, var newTemplates, var updateableTemplates, var onlyLocalTemplates) = Manager.Instance.Templater.CheckForTemplateUpdates();
            var totalTime = DateTime.Now - startTime;
            var totalSeconds = totalTime.TotalSeconds.ToString("0.00");

            // Request what to do based on the results
            if (totalTemplates == -1)
            {
                return "Skipped template update check, rerun command with force flag enabled to force update!";
            }

            _ = LogMessage($"Discovered remote {totalTemplates} template(s) in {totalSeconds} second(s)...");
            if (newTemplates.Count == 0 && updateableTemplates.Count == 0 && onlyLocalTemplates.Count == 0)
            {
                return "No templates to update!";
            }

            if (onlyLocalTemplates.Count > 0)
            {
                // TODO: Allow action to be taken on local-only templates? (delete?)
            }

            var totalTemplatesToDownload = new List<TemplateGitMetadata>();

            if (newTemplates.Count > 0)
            {
                if (Silent || ConsoleHelpers.GetYesNo($"Found {newTemplates.Count} new templates! Queue them for download?", true))
                {
                    totalTemplatesToDownload.AddRange(newTemplates);
                }
            }
            if (updateableTemplates.Count > 0)
            {
                if (Silent || ConsoleHelpers.GetYesNo($"Found {updateableTemplates.Count} out-of-date templates! Queue them for download?", true))
                {
                    totalTemplatesToDownload.AddRange(updateableTemplates);
                }
            }

            if (totalTemplatesToDownload.Count == 0)
            {
                return "No templates queued for download!";
            }

            // Download requested templates
            _ = LogMessage($"Downloading {totalTemplatesToDownload.Count} template(s)...");
            startTime = DateTime.Now;
            Manager.Instance.Templater.DownloadTemplates(totalTemplatesToDownload);
            totalTime = DateTime.Now - startTime;
            totalSeconds = totalTime.TotalSeconds.ToString("0.00");
            return $"Downloaded {totalTemplatesToDownload.Count} template(s) in {totalSeconds} second(s)";
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        /// <param name="option">The option.</param>
        protected override void SetOptions(AbstractOption option)
        {
            var options = (UpdateTemplates)option;
            ForceUpdate = options.ForceUpdate;
            Silent = options.Silent;
        }
    }
}
