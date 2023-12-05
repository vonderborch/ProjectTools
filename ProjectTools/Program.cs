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

        private static void Main(string[] args)
        {
#if DEBUG
            DebugCommands exampleCommands = new();

            var testCommand = ("suggestion", "silent_ready");

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

        private static void ProcessArguments(string[] args)
        {
            // parse command line arguments and execute the appropriate command
            var parseResults = Parser.Default.ParseArguments<Configure, ReportIssue, MakeSuggestion>(args);

            _ = parseResults.MapResult(
                //(Prepare opts) => new Prepare().Execute(opts),
                //(Generate opts) => new Generate().Execute(opts),
                //(ListTemplates opts) => new ListTemplates().Execute(opts),
                (Configure opts) => new Configure().Execute(opts),
                (ReportIssue opts) => new ReportIssue().Execute(opts),
                (MakeSuggestion opts) => new MakeSuggestion().Execute(opts),
                //(UpdateTemplates opts) => new UpdateTemplates().Execute(opts),
                _ => MakeError()
                                      );
        }

        private static void ProcessTestArgument(string suite, string arg, bool hideExceptions = false)
        {
            try
            {
                arg = $"{suite} {arg}";
                var args = arg.Split(" ");
                ProcessArguments(args);
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
