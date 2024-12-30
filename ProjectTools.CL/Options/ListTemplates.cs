using System.Text;
using CommandLine;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to list available templates.
/// </summary>
[Verb("list-templates", HelpText = "Lists all templates available to use.")]
[MenuMetadata(30)]
public class ListTemplates : AbstractOption
{
    /// <summary>
    ///     A flag to indicate whether to output full info or not.
    /// </summary>
    [Option('f', "full", Default = false, Required = false,
        HelpText = "If flag is set, full information on the templates will be displayed.")]
    public bool Full { get; set; }

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        var localTemplates = new LocalTemplates();
        var availableTemplates = localTemplates.Templates;

        if (availableTemplates.Count == 0)
        {
            return "No templates found!";
        }

        StringBuilder output = new();
        if (this.Full)
        {
            foreach (var template in availableTemplates)
            {
                output.AppendLine($"Template: {template.Name}");
                output.AppendLine($"  - Version: {template.Template.Version}");
                output.AppendLine($"  - Author: {template.Template.Author}");
                output.AppendLine($"  - Description: {template.Template.Description}");
            }
        }
        else
        {
            foreach (var template in availableTemplates)
            {
                output.AppendLine($"- {template.Name} - {template.Template.Version}");
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
        var options = (ListTemplates)option;
        this.Full = options.Full;
    }
}
