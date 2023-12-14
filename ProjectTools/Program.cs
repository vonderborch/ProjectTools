using CommandLine;
using ProjectTools.Options;

namespace ProjectTools // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        /// <summary>
        /// Makes the error.
        /// </summary>
        /// <returns></returns>
        public static string MakeError()
        {
            return "\0";
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.ArgumentException">Invalid test command!</exception>
        private static void Main(string[] args)
        {
#if DEBUG
            DebugCommands exampleCommands = new();

            // (string?, string?)
            var testCommand = ("generate", "v_dual");

            // process all commands iteratively
            if (testCommand.Item1 == null && testCommand.Item2 == null)
            {
                foreach (var suite in exampleCommands.PossibleCommands)
                {
                    foreach (var command in suite.Value)
                    {
                        ProcessTestArgument(suite.Key, command.Value, true);
                    }
                }
            }
            // process all commands in grouping iteratively
            else if (testCommand.Item1 != null && testCommand.Item2 == null)
            {
                foreach (var command in exampleCommands.PossibleCommands[testCommand.Item1])
                {
                    ProcessTestArgument(testCommand.Item1, command.Value, true);
                }
            }
            else if (testCommand.Item1 != null && testCommand.Item2 != null)
            {
                ProcessTestArgument(testCommand.Item1, exampleCommands.PossibleCommands[testCommand.Item1][testCommand.Item2]);
            }
            else
            {
                throw new ArgumentException("Invalid test command!");
            }
#else
            ProcessArguments(args);
#endif
        }

        /// <summary>
        /// Processes the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void ProcessArguments(string[] args)
        {
            // parse command line arguments and execute the appropriate command
            var parseResults = Parser.Default.ParseArguments<PrepareTemplate, GenerateProject, ListTemplates, UpdateTemplates, Configure, ReportIssue, MakeSuggestion>(args);

            var result = parseResults.MapResult(
                //(AttachProject opts) => new AttachProject().ExecuteOption(opts),
                (GenerateProject opts) => new GenerateProject().ExecuteOption(opts),
                (PrepareTemplate opts) => new PrepareTemplate().ExecuteOption(opts),
                (ListTemplates opts) => new ListTemplates().ExecuteOption(opts),
                (Configure opts) => new Configure().ExecuteOption(opts),
                (ReportIssue opts) => new ReportIssue().ExecuteOption(opts),
                (MakeSuggestion opts) => new MakeSuggestion().ExecuteOption(opts),
                (UpdateTemplates opts) => new UpdateTemplates().ExecuteOption(opts),
                _ => MakeError()
                                               );

            // print the result if there is one
            if (!string.IsNullOrWhiteSpace(result))
            {
                Console.WriteLine(result);
            }
        }

        /// <summary>
        /// Processes the test argument.
        /// </summary>
        /// <param name="suite">The suite.</param>
        /// <param name="arg">The argument.</param>
        /// <param name="hideExceptions">if set to <c>true</c> [hide exceptions].</param>
        private static void ProcessTestArgument(string suite, string arg, bool hideExceptions = false)
        {
            try
            {
                arg = $"{suite} {arg}";
                var args = arg.Split(" ");
                Console.WriteLine($"COMMAND: {arg}");
                Console.WriteLine(" ");
                Console.WriteLine("----------------------------");
                Console.WriteLine(" ");
                ProcessArguments(args);
                Console.WriteLine(" ");
                Console.WriteLine("----------------------------");
                Console.WriteLine(" ");
            }
            catch (Exception ex)
            {
                if (hideExceptions)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
