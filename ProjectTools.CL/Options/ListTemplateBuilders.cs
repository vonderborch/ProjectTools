using System.Text;
using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to get information about the program with.
/// </summary>
[Verb("list-template-builders", HelpText = "Get information about what template builders are available.")]
public class ListTemplateBuilders : AbstractOption
{
    /// <summary>
    ///     A flag to indicate whether to output full info or not.
    /// </summary>
    [Option('f', "full", Default = false, Required = false,
        HelpText = "If flag is set, full information on the template builders will be displayed.")]
    public bool Full { get; set; }

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        var templater = new Preparer();
        var templaters = templater.GetTemplateBuilders();

        if (templaters.Count == 0)
        {
            return "No template builders found.";
        }

        LogMessage($"Found {templaters.Count} template builders...");
        StringBuilder output = new();
        if (this.Full)
        {
            output.AppendLine("----------------------------------------");
            foreach (var t in templaters)
            {
                output.AppendLine($"Template Builder: {t.Name}");
                output.AppendLine($"  - Version: {t.Version}");
                output.AppendLine($"  - Author: {t.Author}");
                output.AppendLine($"  - Description: {t.Description}");
            }
        }
        else
        {
            foreach (var t in templaters)
            {
                output.AppendLine($"- {t.Name} - {t.Version}");
            }
        }

        return output.ToString();
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (ListTemplateBuilders)option;
        this.Full = options.Full;
    }
}
