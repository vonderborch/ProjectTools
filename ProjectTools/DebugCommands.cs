namespace ProjectTools
{
    internal class DebugCommands
    {
        public Dictionary<string, Dictionary<string, string>> PossibleCommands;

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
