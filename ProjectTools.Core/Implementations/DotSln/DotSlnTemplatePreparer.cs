using System.Text;
using System.Text.RegularExpressions;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Core.Templating.Preparation;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// Defines a template preparer for .sln projects/solutions
    /// </summary>
    /// <seealso cref="ProjectTools.Core.Templating.Preparation.AbstractTemplatePreparer"/>
    public class DotSlnTemplatePreparer : AbstractTemplatePreparer
    {
        /// <summary>
        /// The files to update
        /// </summary>
        private static readonly string[] _files_to_update =
        {
            ".sln",
            ".csproj",
            ".shproj",
            ".projitems"
        };

        /// <summary>
        /// The regex tags
        /// </summary>
        private static readonly string[][] _regex_tags =
        {
            ["Authors", DotSlnConstants.REGEX_TAGS[0]],
            ["Company", DotSlnConstants.REGEX_TAGS[1]],
            ["PackageTags", DotSlnConstants.REGEX_TAGS[2]],
            ["Description", DotSlnConstants.REGEX_TAGS[3]],
            ["PackageLicenseExpression", DotSlnConstants.REGEX_TAGS[4]],
            ["Version", DotSlnConstants.REGEX_TAGS[5]],
            ["FileVersion", DotSlnConstants.REGEX_TAGS[5]],
            ["AssemblyVersion", DotSlnConstants.REGEX_TAGS[5]]
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DotSlnTemplatePreparer"/> class.
        /// </summary>
        public DotSlnTemplatePreparer() : base("VisualStudio (.sln)", Implementation.DotSln) { }

        /// <summary>
        /// Checks whether the directory is valid for this templater.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>True if valid, False otherwise.</returns>
        public override bool DirectoryValidForTemplater(string directory)
        {
            // check if any file is a .sln file...
            foreach (var file in System.IO.Directory.GetFiles(directory))
            {
                if (file.EndsWith(".sln"))
                {
                    return true;
                }
            }

            foreach (var subDirectory in System.IO.Directory.GetDirectories(directory))
            {
                var result = DirectoryValidForTemplater(subDirectory);
                if (result)
                {
                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the guids.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>All guids for all .sln files in the directory we're templating.</returns>
        public Dictionary<string, string> GetGuids(string directory, int guidCount = 0)
        {
            Dictionary<string, string> output = new();

            // try to find .sln files
            var files = Directory.GetFiles(directory);
            for (var i = 0; i < files.Length; i++)
            {
                if (Path.GetExtension(files[i]) == ".sln")
                {
                    var lines = File.ReadAllLines(files[i]);

                    foreach (var line in lines)
                    {
                        if (
                            line.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase)
                           )
                        {
                            var splitByComma = line.Split(",");
                            var last = splitByComma[splitByComma.Length - 1];

                            var guid = last.Substring(3, last.Length - 5);
                            output.Add(guid, $"GUID{guidCount.ToString(Constants.GUID_PADDING)}");
                            guidCount++;
                        }
                    }
                }
            }

            // look through sub-directories
            var directories = Directory.GetDirectories(directory);
            for (var i = 0; i < directories.Length; i++)
            {
                var directoryGuids = GetGuids(directories[i], output.Count);
                foreach (var guid in directoryGuids)
                {
                    output.Add(guid.Key, guid.Value);
                }
            }

            return output;
        }

        public override TemplateSettings GetSettingsClassForProperties(Dictionary<SettingProperty, object> settings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the template settings class.
        /// </summary>
        /// <returns>The template settings class defined for this preparer.</returns>
        public override Type? GetTemplateSettingsClass()
        {
            return typeof(DotSlnTemplateSettings);
        }

        /// <summary>
        /// Prepares the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="log"></param>
        /// <returns>The preperation result.</returns>
        public override string Prepare(PrepareOptions options, Func<string, bool> log)
        {
            var templateSettings = (DotSlnTemplate)options.TemplateSettings;
            var startTime = DateTime.Now;
            var directoryName = options.TemplateSettings.Name.Replace(" ", "_");
            var workingDirectory = Path.Combine(options.OutputDirectory, directoryName);
            var archivePath = Path.Combine(
                options.OutputDirectory,
                $"{directoryName}.{Constants.TemplateFileType}"
                                          );

            // Step 1 - Create a template_info.json file if one doesn't exist. Or update it
            log("Generating/updating template_info.json file...");
            var baseTemplateInfoFile = Path.Combine(
                options.Directory,
                Constants.TemplaterTemplatesInfoFileName
                                                   );
            DeleteFileIfExists(baseTemplateInfoFile);
            var serialized = templateSettings.ToJson();
            File.WriteAllText(baseTemplateInfoFile, serialized);
            log("  template_info.json file generated/updated!");

            // Step 2 - Delete the working directory if it already exists
            log($"Checking if working directory '{workingDirectory}' exists...");
            if (DeleteDirectoryIfExists(workingDirectory))
            {
                log("  Deleted existing directory!");
            }
            else
            {
                log("  Directory does not exist!");
            }

            // Step 3 - Copy the source directory to the working directory
            log(
                $"Copying base solution '{options.Directory}' to working direcotry '{workingDirectory}'..."
               );
            CopyDirectory(
                options.Directory,
                workingDirectory,
                options.TemplateSettings.Settings.DirectoriesExcludedInPrepare
                         );
            log("  Base solution copied!");

            // Step 4 - Get Guids
            log("Getting GUIDs in solution...");
            var guids = GetGuids(workingDirectory);
            log($"  Found {guids.Count} GUIDs!");

            // Step 5 - Update template_info.json
            log("Updating template_info.json with GUID count...");
            var templateSettingsFile = Path.Combine(
                workingDirectory,
                Constants.TemplaterTemplatesInfoFileName
                                                   );
            templateSettings.GuidCount = guids.Count;
            serialized = templateSettings.ToJson();
            File.WriteAllText(templateSettingsFile, serialized);
            log("  template_info.json updated!");

            // Step 6 - Update any solutions in the working directory
            log("Updating template with generic replacement text...");
            PrepDirectory(workingDirectory, templateSettings, guids);
            log("  Template updated!");

            // Step 7 - Archive the Directory and Delete the working directory
            log("Packaging template...");
            ArchiveDirectory(workingDirectory, archivePath, options.SkipCleaning);
            log("  Template packaged!");

            // Step 8 - DONE!
            log("Work complete!");
            var totalTime = DateTime.Now - startTime; // dpesn't have to be perfect, this should be fine!
            return $"Successfully prepared the template in {totalTime.TotalSeconds.ToString("0.00")} second(s): {archivePath}";
        }

        /// <summary>
        /// Preps the directory to be a template.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="guids">The guids.</param>
        public void PrepDirectory(
            string directory,
            DotSlnTemplate settings,
            Dictionary<string, string> guids
                                 )
        {
            // Get Replacement Text for the Template and Update the Files
            var replacements = GetReplacementText(guids, settings);
            UpdateFiles(directory, replacements.Item1, replacements.Item2);
        }

        /// <summary>
        /// Gets the replacement text for files.
        /// </summary>
        /// <param name="guids">The guids.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Item 1- Solution File Replacements | Item 2- Other File Replacements</returns>
        private (Dictionary<string, string>, Dictionary<Regex, string>) GetReplacementText(
            Dictionary<string, string> guids,
            DotSlnTemplate settings
                                                                                          )
        {
            // Solution File Text Replacements
            var slnReplacements = new Dictionary<string, string>(guids);
            foreach (var replacement in settings.Settings.ReplacementText)
            {
                slnReplacements.Add(replacement.Item1, replacement.Item2);
            }

            // Other file replacements
            Dictionary<Regex, string> otherReplacements = new();
            for (var i = 0; i < _regex_tags.Length; i++)
            {
                otherReplacements.Add(
                    new Regex($"<{_regex_tags[i][0]}>.*<\\/{_regex_tags[i][0]}>"),
                    $"<{_regex_tags[i][0]}>{_regex_tags[i][1]}</{_regex_tags[i][0]}>"
                                     );
            }

            foreach (var replacement in settings.Settings.ReplacementText)
            {
                otherReplacements.Add(new Regex(replacement.Item1), replacement.Item2);
            }

            // return! (just in case someone needed help here)
            return (slnReplacements, otherReplacements);
        }

        /// <summary>
        /// Updates the files.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="slnReplacements">The SLN replacements.</param>
        /// <param name="otherReplacements">The other replacements.</param>
        private void UpdateFiles(
            string directory,
            Dictionary<string, string> slnReplacements,
            Dictionary<Regex, string> otherReplacements
                                )
        {
            var files = Directory
                .GetFiles(directory)
                .Where(
                    f =>
                        _files_to_update.Contains(Path.GetExtension(f))
                        && Path.GetFileName(f) != Constants.TemplaterTemplatesInfoFileName
                      )
                .ToList();

            for (var i = 0; i < files.Count; i++)
            {
                var fileContents = File.ReadAllLines(files[i]);
                // if it is a .sln file...
                if (Path.GetExtension(files[i]) == ".sln")
                {
                    File.WriteAllText(files[i], UpdateSolutionFile(fileContents, slnReplacements));
                }
                // otherwise...
                else
                {
                    File.WriteAllText(files[i], UpdateOtherFile(fileContents, otherReplacements));
                }
            }

            // Update csproj files in sub-directories
            var directories = Directory.GetDirectories(directory);
            for (var i = 0; i < directories.Length; i++)
            {
                UpdateFiles(directories[i], slnReplacements, otherReplacements);
            }
        }

        /// <summary>
        /// Updates the other file.
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns>The updated file contents.</returns>
        private string UpdateOtherFile(
            string[] fileContents,
            Dictionary<Regex, string> replacements
                                      )
        {
            var output = new StringBuilder();

            for (var i = 0; i < fileContents.Length; i++)
            {
                var textLine = fileContents[i];
                foreach (var replacement in replacements)
                {
                    textLine = replacement.Key.Replace(textLine, replacement.Value);
                }

                output.AppendLine(textLine);
            }

            return output.ToString();
        }

        /// <summary>
        /// Updates the solution file.
        /// </summary>
        /// <param name="fileContents">The file contents.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns>The updted file contents.</returns>
        private string UpdateSolutionFile(
            string[] fileContents,
            Dictionary<string, string> replacements
                                         )
        {
            var mergedContents = string.Join(Environment.NewLine, fileContents);

            foreach (var pair in replacements)
            {
                mergedContents = mergedContents.Replace(pair.Key, pair.Value);
            }

            return mergedContents;
        }
    }
}
