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
        
        // Configuration Commands
        
        // About Commands
        
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
