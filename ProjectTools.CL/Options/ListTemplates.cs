using CommandLine;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to list available templates.
/// </summary>
[Verb("list-templates", HelpText = "Lists all templates available to use.")]
public class ListTemplates : AbstractOption
{
    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        // TODO - Add logic to list all templates

        return "";
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
    }
}
