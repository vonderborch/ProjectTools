using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core;
using ProjectTools.Core.TemplateRepositories;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to get information about the program with.
/// </summary>
[Verb("update-templates", HelpText = "Lists all templates available to use.")]
[MenuMetadata(20, "-f")]
public class UpdateTemplates : SilenceAbleAbstractOption
{
    /// <summary>
    ///     Creates a new instance of the <see cref="UpdateTemplates" /> class.
    /// </summary>
    public UpdateTemplates()
    {
        this.AllowTemplateUpdates = false;
    }

    /// <summary>
    ///     A flag indicating whether to force-check for new templates and ignore the cached data.
    /// </summary>
    [Option('f', "force-check", Required = false, Default = false,
        HelpText = "If flag is provided, the program will force-check for new templates and ignore the cached data.")]
    public bool ForceCheck { get; set; }

    /// <summary>
    ///     A flag indicating whether to force re-download all templates.
    /// </summary>
    [Option('u', "force-redownload", Required = false, Default = false,
        HelpText = "If flag is provided, the program will force all templates to re-download.")]
    public bool ForceRedownload { get; set; }

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        // Step 1 - Check for updates...
        LogMessage("Checking for new or updated templates...");
        var startTime = DateTime.Now;
        var updateResultCheck = TemplateUpdater.CheckForTemplateUpdates(forceCheck: this.ForceCheck);
        var updateResultCheckTotalSeconds = TotalSecondsSinceTime(startTime);

        // Exit early if we skipped the update check...
        if (updateResultCheck.TotalRemoteTemplatesProcessed == -1)
        {
            return "Skipped template update check due to recent check!";
        }

        // Exit early if there are no templates to update or download...
        LogMessage(
            $"Discovered {updateResultCheck.TotalRemoteTemplatesProcessed} remote template(s) in {updateResultCheckTotalSeconds} second(s)...");
        if (updateResultCheck.TotalTemplatesNeedingDownload == 0)
        {
            return "No templates to update or download!";
        }

        // Step 2 - Take Action on Orphaned Templates...
        //// TODO - Allow action to be taken on local-only templates? (delete?)
        LogMessage($"Found {updateResultCheck.OrphanedTemplates.Count} local-only templates...");

        // Step 3 - Ask user what templates to download...
        var totalTemplatesToDownload = new List<GitTemplateMetadata>();
        var totalDownloadingSize = 0UL;
        if (updateResultCheck.TotalTemplatesNeedingDownload > 0)
        {
            LogMessage($"Found {updateResultCheck.TotalTemplatesNeedingDownload} templates to download/update...");
            // Step 3a - Ask about new templates...
            foreach (var template in updateResultCheck.NewTemplates)
            {
                if (this.Silent ||
                    ConsoleHelpers.GetYesNo(
                        $"Download new template `{template.DisplayName}` ({template.SizeInMb:0.000} megabyte(s))?"))
                {
                    totalDownloadingSize += (ulong)template.Size;
                    totalTemplatesToDownload.Add(template);
                }
            }

            // Step 3b - Ask about updateable templates...
            foreach (var template in updateResultCheck.UpdateableTemplates)
            {
                if (this.Silent ||
                    ConsoleHelpers.GetYesNo(
                        $"Update template `{template.DisplayName}` ({template.SizeInMb:0.000} megabyte(s))?"))
                {
                    totalDownloadingSize += (ulong)template.Size;
                    totalTemplatesToDownload.Add(template);
                }
            }
        }
        else
        {
            return "Found no templates to download or update!";
        }

        // Exit early if no templates are queued for download...
        if (totalTemplatesToDownload.Count == 0)
        {
            return "No templates queued for download!";
        }

        // Step 4 - Download the requested templates...
        var totalDownloadingSizeMegabytes = totalDownloadingSize / 1024d / 1024d;
        LogMessage(
            $"Downloading {totalTemplatesToDownload.Count} template(s) ({totalDownloadingSizeMegabytes:0.000} megabyte(s))...");
        startTime = DateTime.Now;
        TemplateUpdater.DownloadTemplates(totalTemplatesToDownload, true);
        return
            $"Downloaded {totalTemplatesToDownload.Count} template(s) in {TotalSecondsSinceTime(startTime)} second(s)";
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (UpdateTemplates)option;
        this.Silent = options.Silent;
        this.ForceCheck = options.ForceCheck;
        this.ForceRedownload = options.ForceRedownload;
    }
}
