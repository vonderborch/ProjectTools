using System.Diagnostics;
using System.Reflection;
using CommandLine;
using ProjectTools.Core.Constants;

namespace ProjectTools.CL.Options;

[Verb("about", HelpText = "Get information about the program.")]
public class About : AbstractOption
{
    public override string Execute()
    {
        Console.WriteLine("Program: ProjectTools");
        Console.WriteLine($"Version: {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}");
        Console.WriteLine($"Core Version: {AppConstants.CoreVersion}");
        Console.WriteLine("Author: Christian Webber");
        
        return "";
    }

    protected override void SetOptions(AbstractOption option)
    {
        var options = (About)option;
        Silent = options.Silent;
    }
}
