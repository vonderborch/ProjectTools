namespace ProjectTools.CL;

internal class Program
{
    /// <summary>
    ///     Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <exception cref="System.ArgumentException">Invalid test command!</exception>
    private static void Main(string[] args)
    {
        var commandHelper = new CommandHelper();

#if DEBUG
        var testRootCommand = "prepare";
        var testCommandSpecific = "dual";

        args = new DebugCommands(commandHelper).GetArgumentsForCommandToRun(testRootCommand, testCommandSpecific);
#endif

        commandHelper.ParseAndExecuteArguments(args);
    }
}
