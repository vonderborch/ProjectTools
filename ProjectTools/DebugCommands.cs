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

            //// Add Configure Example Commands
            PossibleCommands.Add("configure", []);
            PossibleCommands["configure"].Add("base", "");

            //// Add Report Issue Example Commands
            PossibleCommands.Add("report-issue", []);
            PossibleCommands["report-issue"].Add("base", "");
            PossibleCommands["report-issue"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["report-issue"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["report-issue"].Add("silent_bad", "-s");

            //// Add Make Suggestion Example Commands
            PossibleCommands.Add("suggestion", []);
            PossibleCommands["suggestion"].Add("base", "");
            PossibleCommands["suggestion"].Add("prepopulated_short", "-t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["suggestion"].Add("silent_ready", "-s -t MyTitleHere -d MyDescriptionHere");
            PossibleCommands["suggestion"].Add("silent_bad", "-s");

            //// List Template Example Commands
            PossibleCommands.Add("list-templates", []);
            PossibleCommands["list-templates"].Add("base", "");
            PossibleCommands["list-templates"].Add("quick_info", "-q");

            //// Check for Updated Templates Commands
            PossibleCommands.Add("update-templates", []);
            PossibleCommands["update-templates"].Add("base", "");
            PossibleCommands["update-templates"].Add("ignore_cache", "-i");
            PossibleCommands["update-templates"].Add("force_updates", "-f");
            PossibleCommands["update-templates"].Add("force_updates_silent", "-f -s");

            //// Prepare Template Example Commands
            PossibleCommands.Add("prepare", []);
            var currentDirectory = Directory.GetCurrentDirectory();
            var baseDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..", "..", "Templates"));
            var outputDirectory = Path.Combine(baseDirectory, "TEMPLATES");
            List<(string, string)> projects = [("v_base", "Velentr.BASE"), ("v_dual", "Velentr.DUAL_SUPPORT"), ("v_dual_gen", "Velentr.DUAL_SUPPORT_WITH_GENERIC")];

            foreach ((var name, var project) in projects)
            {
                var projectDirectory = Path.Combine(baseDirectory, "TEMPLATES_BASE", project);
                var command = $"-d {projectDirectory} -o {outputDirectory}";

                PossibleCommands["prepare"].Add(name, command);
            }

            //// Generate Project Example Commands
            PossibleCommands.Add("generate", []);
            var generatedOutputDirectory = Path.Combine(currentDirectory, "GENERATED");

            foreach ((var name, var project) in projects)
            {
                var projectDirectory = Path.Combine(baseDirectory, "TEMPLATES_BASE", project);
                var command = $"-f -n {name} -t {project} -o {generatedOutputDirectory}";

                PossibleCommands["generate"].Add(name, command);
            }
        }
    }
}

#endif
