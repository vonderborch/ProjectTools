#if DEBUG

namespace ProjectTools
{
    /// <summary>
    /// A class used during development to test the command line arguments.
    /// </summary>
    internal class DebugCommands
    {
        /// <summary>
        /// The possible commands
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> PossibleCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugCommands"/> class.
        /// </summary>
        public DebugCommands()
        {
            PossibleCommands = [];

            // Add Configure Example Commands
            PossibleCommands.Add("configure", []);
            PossibleCommands["configure"].Add("base", "");

            // Add Report Issue Example Commands
            PossibleCommands.Add("report-issue", []);
            PossibleCommands["report-issue"].Add("base", "");
            PossibleCommands["report-issue"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["report-issue"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["report-issue"].Add("silent_bad", "-s");

            // Add Make Suggestion Example Commands
            PossibleCommands.Add("suggestion", []);
            PossibleCommands["suggestion"].Add("base", "");
            PossibleCommands["suggestion"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["suggestion"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["suggestion"].Add("silent_bad", "-s");
        }
    }
}

#endif
