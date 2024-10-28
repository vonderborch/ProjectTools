using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to get information about the program with.
/// </summary>
[Verb("list-templaters", HelpText = "Get information about what templaters are available.")]
public class ListTemplaters : AbstractOption
{
    [Option('i', "simple", Default = false, Required = false,
        HelpText = "Display just the name and version of available templaters.")]
    public bool Simple { get; set; }

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        var templater = new Templater();
        var templaters = templater.GetTemplaters();

        if (templaters.Count == 0)
        {
            Console.WriteLine("No templaters found.");
        }
        else
        {
            Console.WriteLine($"Found {templaters.Count} templaters...");
            if (Simple)
            {
                foreach (var t in templaters) Console.WriteLine($"- {t.Name} - {t.Version}");
            }
            else
            {
                Console.WriteLine("----------------------------------------");
                foreach (var t in templaters)
                {
                    Console.WriteLine($"Templater: {t.Name}");
                    Console.WriteLine($"  - Version: {t.Version}");
                    Console.WriteLine($"  - Author: {t.Author}");
                    Console.WriteLine($"  - Description: {t.Description}");
                }
            }
        }

        return "";
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (ListTemplaters)option;
        Silent = options.Silent;
        Simple = options.Simple;
    }
}
