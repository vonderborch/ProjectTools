namespace ProjectTools.CL;

public class DebugCommands
{
    private Dictionary<string, Dictionary<string, string>> _commands;

    public DebugCommands(CommandHelper commandHelper)
    {
        _commands = new();
        var baseCommands = commandHelper.GetAvailableRootCommands();
        foreach (var command in baseCommands)
        {
            _commands.Add(command, []);
            _commands[command].Add("", "");
        }

        //// Configuration Commands

        //// About Commands

        //// Add Report Issue Example Commands
        _commands["report-issue"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
        _commands["report-issue"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
        _commands["report-issue"].Add("silent_bad", "-s");

        //// Add Make Suggestion Example Commands
        _commands["suggestion"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
        _commands["suggestion"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
        _commands["suggestion"].Add("silent_bad", "-s");

        //// Add List Template Builders Example Commands
        _commands["list-template-builders"].Add("simple", "-i");
    }

    public string[] GetArgumentsForCommandToRun(string rootCommand, string specificCommand)
    {
        if (!_commands.ContainsKey(rootCommand))
        {
            throw new ArgumentException($"Invalid root command: {rootCommand}");
        }

        if (!_commands[rootCommand].ContainsKey(specificCommand))
        {
            throw new ArgumentException($"Invalid specific command: {specificCommand}");
        }

        var specificCommandArguments = _commands[rootCommand][specificCommand];
        var finalArgs = new List<string>();
        finalArgs.Add(rootCommand);
        finalArgs.AddRange(specificCommandArguments.Split(" "));
        return finalArgs.ToArray();
    }
}
