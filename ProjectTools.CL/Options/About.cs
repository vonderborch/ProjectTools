using System.Diagnostics;
using System.Reflection;
using CommandLine;
using ProjectTools.Core.Constants;

namespace ProjectTools.CL.Options;

/// <summary>
/// A command to get information about the program with.
/// </summary>
[Verb("about", HelpText = "Get information about the program.")]
public class About : AbstractOption
{
    /// <summary>
    /// Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        Console.WriteLine("Program: ProjectTools");
        Console.WriteLine($"Version: {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}");
        Console.WriteLine($"Core Version: {AppConstants.CoreVersion}");
        Console.WriteLine("Author: Christian Webber");
        
        return "";
    }

    /// <summary>
    /// Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (About)option;
        Silent = options.Silent;
    }
}