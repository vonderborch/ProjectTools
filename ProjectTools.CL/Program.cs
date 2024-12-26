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
        var testRootCommand = "";
        var testCommandSpecific = "";

        args = new DebugCommands(commandHelper).GetArgumentsForCommandToRun(testRootCommand, testCommandSpecific);
#endif
        var useMenu = args.Length == 0;

        do
        {
            if (useMenu)
            {
                var menuItem = commandHelper.DisplayMenu();
                if (menuItem == "EXIT")
                {
                    return;
                }

                args = menuItem.Split(" ").ToArray();
                Console.WriteLine(" ");
            }

            commandHelper.ParseAndExecuteArguments(args);
            if (useMenu)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        } while (useMenu);
    }
}
