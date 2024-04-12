using System.Text;
using System.Text.RegularExpressions;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Preperation;

namespace ProjectTools.Core.Implementations.DotSln;

/// <summary>
/// A helper class to store logic for preparing a Visual Studio (.sln) solution for templating
/// </summary>
internal class DotSlnSolutionPreparer : AbstractPreparer
{
    /// <summary>
    /// The files to update
    /// </summary>
    private static readonly string[] _files_to_update = { ".sln", ".csproj", ".shproj", ".projitems" };

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
    /// Initializes a new instance of the <see cref="DotSlnSolutionPreparer"/> class.
    /// </summary>
    /// <param name="templater">The templater.</param>
    /// <param name="log">The log.</param>
    public DotSlnSolutionPreparer(Func<string, bool> log) : base(log)
    {
    }

    /// <summary>
    /// Prepares the template.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns>The result of the preperation</returns>
    public override string PrepareTemplate(PrepareOptions options)
    {
        // setup some variables...
        var startTime = DateTime.Now;

        var initialSize = GetDirectorySize(options.Directory) / 1000000f; // get size in MB
        var templateInformation = options.Template.Information;
        var templateSettings = (DotSlnTemplateSettings)options.Template.Settings;

        var directoryName = templateInformation.Name.Replace(" ", "_");
        var workingDirectory = Path.Combine(options.OutputDirectory, directoryName);
        var archivePath = Path.Combine(options.OutputDirectory, $"{directoryName}.{Constants.TemplateFileType}");

        // Step 1 - Delete the working directory if it already exists
        Logger.Log($"Step 1/5: Checking if working directory '{workingDirectory}' exists...");
        var exists = Directory.Exists(workingDirectory);
        IOHelpers.DeleteDirectoryIfExists(workingDirectory);
        Logger.Log(exists ? "Deleted existing directory!" : "Directory does not exist!", 1);

        // Step 2 - Copy the source directory to the working directory
        Logger.Log(
            $"Step 2/5: Copying base solution '{options.Directory}' to working direcotry '{workingDirectory}'...");
        IOHelpers.CopyDirectory(options.Directory, workingDirectory, templateSettings.DirectoriesExcludedInPrepare);
        Logger.Log("Base solution copied!", 1);

        // Step 3 - Get Guids
        // NOTE: we aren't going to be storing these anymore. Instead, GUIDs will be automatically generated
        // on-the-fly by the templater as required on generation
        Logger.Log("Step 3/5: Getting GUIDs in solution...");
        var guids = GetGuids(workingDirectory);
        Logger.Log($"Found {guids.Count} GUIDs!", 1);

        // Step 4 - Update any solutions in the working directory
        Logger.Log("Step 4/5: Updating template with generic replacement text...");
        PrepDirectory(workingDirectory, templateSettings, guids);
        Logger.Log("Template updated!", 1);

        // Step 5 - Archive the Directory and Delete the working directory
        Logger.Log("Step 5/5: Packaging template...");
        IOHelpers.ArchiveDirectory(workingDirectory, archivePath, options.SkipCleaning);
        Logger.Log("Template packaged!", 1);

        // DONE!!!
        Logger.Log("Work complete!");
        var fileSize = new FileInfo(archivePath).Length / 1000000f; // get size in MB
        Logger.Log($"Original Solution Size: {initialSize:0.000} mb", 1);
        Logger.Log($"Template Size: {fileSize:0.000} mb", 1);
        var totalTime = DateTime.Now - startTime; // dpesn't have to be perfect, this should be fine!
        return $"Successfully prepared the template in {totalTime.TotalSeconds:0.00} second(s): {archivePath}";
    }

    /// <summary>
    /// Gets the size of the directory.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The size in bytes</returns>
    private static ulong GetDirectorySize(string path)
    {
        var size = 0UL;

        var baseDir = new DirectoryInfo(path);
        foreach (var file in baseDir.GetFiles()) size += (ulong)file.Length;
        foreach (var dir in baseDir.GetDirectories()) size += GetDirectorySize(dir.FullName);

        return size;
    }

    /// <summary>
    /// Gets the guids.
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <returns>All guids for all .sln files in the directory we're templating.</returns>
    private Dictionary<string, string> GetGuids(string directory, int guidCount = 0)
    {
        Dictionary<string, string> output = [];

        // try to find .sln files
        var files = Directory.GetFiles(directory);
        for (var i = 0; i < files.Length; i++)
            if (Path.GetExtension(files[i]) == ".sln")
            {
                var lines = File.ReadAllLines(files[i]);

                foreach (var line in lines)
                    if (line.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var splitByComma = line.Split(",");
                        var last = splitByComma[^1];

                        var guid = last[3..^2];
                        output.Add(guid, $"GUID{guidCount.ToString(Constants.GUID_PADDING)}");
                        guidCount++;
                    }
            }

        // look through sub-directories
        var directories = Directory.GetDirectories(directory);
        for (var i = 0; i < directories.Length; i++)
        {
            var directoryGuids = GetGuids(directories[i], output.Count);
            foreach (var guid in directoryGuids) output.Add(guid.Key, guid.Value);
        }

        return output;
    }

    /// <summary>
    /// Gets the replacement text for files.
    /// </summary>
    /// <param name="guids">The guids.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>Item 1- Solution File Replacements | Item 2- Other File Replacements</returns>
    private (Dictionary<string, string>, Dictionary<Regex, string>) GetReplacementText(Dictionary<string, string> guids,
        DotSlnTemplateSettings settings)
    {
        // Solution File Text Replacements
        var slnReplacements = new Dictionary<string, string>(guids);
        foreach (var replacement in settings.ReplacementText) slnReplacements.Add(replacement.Key, replacement.Value);

        // Other file replacements
        Dictionary<Regex, string> otherReplacements = [];
        for (var i = 0; i < _regex_tags.Length; i++)
            otherReplacements.Add(
                new Regex($"<{_regex_tags[i][0]}>.*<\\/{_regex_tags[i][0]}>"),
                $"<{_regex_tags[i][0]}>{_regex_tags[i][1]}</{_regex_tags[i][0]}>"
            );

        foreach (var replacement in settings.ReplacementText)
            otherReplacements.Add(new Regex(replacement.Key), replacement.Value);

        // return! (just in case someone needed help here)
        return (slnReplacements, otherReplacements);
    }

    /// <summary>
    /// Preps the directory to be a template.
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="guids">The guids.</param>
    private void PrepDirectory(string directory, DotSlnTemplateSettings settings, Dictionary<string, string> guids)
    {
        // Get Replacement Text for the Template and Update the Files
        var replacements = GetReplacementText(guids, settings);
        UpdateFiles(directory, replacements.Item1, replacements.Item2);
    }

    /// <summary>
    /// Updates the files.
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <param name="slnReplacements">The SLN replacements.</param>
    /// <param name="otherReplacements">The other replacements.</param>
    private void UpdateFiles(string directory, Dictionary<string, string> slnReplacements,
        Dictionary<Regex, string> otherReplacements)
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
                File.WriteAllText(files[i], UpdateSolutionFile(fileContents, slnReplacements));
            // otherwise...
            else
                File.WriteAllText(files[i], UpdateOtherFile(fileContents, otherReplacements));
        }

        // Update csproj files in sub-directories
        var directories = Directory.GetDirectories(directory);
        for (var i = 0; i < directories.Length; i++) UpdateFiles(directories[i], slnReplacements, otherReplacements);
    }

    /// <summary>
    /// Updates the other file.
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="replacements">The replacements.</param>
    /// <returns>The updated file contents.</returns>
    private string UpdateOtherFile(string[] fileContents, Dictionary<Regex, string> replacements)
    {
        var output = new StringBuilder();

        for (var i = 0; i < fileContents.Length; i++)
        {
            var textLine = fileContents[i];
            foreach (var replacement in replacements) textLine = replacement.Key.Replace(textLine, replacement.Value);

            _ = output.AppendLine(textLine);
        }

        return output.ToString();
    }

    /// <summary>
    /// Updates the solution file.
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="replacements">The replacements.</param>
    /// <returns>The updted file contents.</returns>
    private string UpdateSolutionFile(string[] fileContents, Dictionary<string, string> replacements)
    {
        var mergedContents = string.Join(Environment.NewLine, fileContents);

        foreach (var pair in replacements) mergedContents = mergedContents.Replace(pair.Key, pair.Value);

        return mergedContents;
    }
}