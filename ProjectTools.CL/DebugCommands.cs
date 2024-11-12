using System.Reflection;

namespace ProjectTools.CL;

public class DebugCommands
{
    private readonly Dictionary<string, Dictionary<string, string>> _commands;

    public DebugCommands(CommandHelper commandHelper)
    {
        //// Directory Info
        var currentDirectory = Assembly.GetExecutingAssembly().Location;
        var testOutputDirectory = Path.Combine(currentDirectory, "TEST_OUTPUT_DIRECTORY");
        var rootRepoDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\"));
        var templatesDirectory = Path.Combine(rootRepoDirectory, "Templates", "TEMPLATES_BASE");
        var templatesOutputDirectory = Path.Combine(rootRepoDirectory, "Templates", "TEMPLATES");

        //// Prep base base commands
        this._commands = new Dictionary<string, Dictionary<string, string>>();
        var baseCommands = commandHelper.GetAvailableRootCommands();
        foreach (var command in baseCommands)
        {
            this._commands.Add(command, []);
            this._commands[command].Add("", "");
        }

        //// Configuration Commands

        //// About Commands

        //// Add Report Issue Example Commands
        this._commands["report-issue"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
        this._commands["report-issue"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
        this._commands["report-issue"].Add("silent_bad", "-s");

        //// Add Make Suggestion Example Commands
        this._commands["suggestion"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
        this._commands["suggestion"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
        this._commands["suggestion"].Add("silent_bad", "-s");

        //// Add List Template Builders Example Commands
        this._commands["list-template-builders"].Add("full", "-f");

        //// Add List Templates Example Commands
        this._commands["list-templates"].Add("full", "-f");

        //// Add update-templates Example Commands
        this._commands["update-templates"].Add("full", "-f -u");
        this._commands["update-templates"].Add("force_check", "-f");
        this._commands["update-templates"].Add("force_redownload", "-i");

        //// Prepare Commands
        var baseTemplate = Path.Combine(templatesDirectory, "Velentr.BASE");
        var dualTemplate = Path.Combine(templatesDirectory, "Velentr.DUAL_SUPPORT");
        var dualAndGenericTemplate = Path.Combine(templatesDirectory, "Velentr.DUAL_SUPPORT_WITH_GENERIC");
        this._commands["prepare"].Add("base", $"-f -d {baseTemplate} -o {templatesOutputDirectory}");
        this._commands["prepare"].Add("dual", $"-f -d {dualTemplate} -o {templatesOutputDirectory}");
        this._commands["prepare"]
            .Add("dual_and_generic", $"-f -d {dualAndGenericTemplate} -o {templatesOutputDirectory}");

        //// Add Generate-Project Commands
        this._commands["generate"].Add("base", $"-f -o {testOutputDirectory} -t Velentr.BASE");
    }

    public string[] GetArgumentsForCommandToRun(string rootCommand, string specificCommand)
    {
        if (!this._commands.ContainsKey(rootCommand))
        {
            throw new ArgumentException($"Invalid root command: {rootCommand}");
        }

        if (!this._commands[rootCommand].ContainsKey(specificCommand))
        {
            throw new ArgumentException($"Invalid specific command: {specificCommand}");
        }

        var specificCommandArguments = this._commands[rootCommand][specificCommand];
        var finalArgs = new List<string>();
        finalArgs.Add(rootCommand);
        finalArgs.AddRange(specificCommandArguments.Split(" "));
        return finalArgs.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
    }
}
