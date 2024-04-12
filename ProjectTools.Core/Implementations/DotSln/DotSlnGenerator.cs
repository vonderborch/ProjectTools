using System.Text;
using Octokit;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Core.Templating.Generation;

namespace ProjectTools.Core.Implementations.DotSln;

internal class DotSlnGenerator : AbstractGenerator
{
    public DotSlnGenerator(Func<string, bool> log, Func<string, bool> instructionLog, Func<string, bool> commandLog) :
        base(log, instructionLog, commandLog)
    {
    }

    public override string GenerateProject(GenerateOptions options)
    {
        var startTime = DateTime.Now;
        var settings = (DotSlnSolutionSettings)options.SolutionSettings;
        var template = Manager.Instance.Templater.GetTemplateByName(options.Template);
        var directoryName = options.SolutionSettings.Name;
        var actualDirectory = Path.Combine(options.OutputDirectory, directoryName);

        // Step 1 - Check if the directory already exists and return if it does (or delete if we're allowed)
        Logger.Log($"Step 1/7: Checking if the new solution directory '{actualDirectory}' already exists...");
        if (Directory.Exists(actualDirectory))
        {
            if (options.Force)
            {
                Logger.Log($"Deleting existing directory...", 1);
                IOHelpers.DeleteDirectoryIfExists(actualDirectory);
                Logger.Log($"Directory deleted!", 1);
            }
            else
            {
                Logger.Log(
                    $"Directory {actualDirectory} already exists. Use the -f flag to force the generation of the solution.",
                    1);
                return string.Empty;
            }
        }

        // Step 2 - Directory creation
        Logger.Log("Step 2/7: Directory creation...");
        if (options.SolutionSettings.GitRepoMode != GitRepoMode.NoRepo)
        {
            var gitUrl =
                $"https://github.com/{options.SolutionSettings.GitRepoOwner}/{options.SolutionSettings.GitRepoName}.git";
            switch (options.SolutionSettings.GitRepoMode)
            {
                case GitRepoMode.NewRepoOnlyInit:
                    Logger.Log($"Initializing Git Repo at destination directory '{actualDirectory}' ...", 1);
                    _ = Directory.CreateDirectory(actualDirectory);
                    _ = LibGit2Sharp.Repository.Init(actualDirectory);
                    Logger.Log("Directory initialized!", 2);
                    break;

                case GitRepoMode.NewRepoFull:
                    Logger.Log($"Creating git repo: {gitUrl} ...", 1);
                    var repository = new NewRepository(options.SolutionSettings.GitRepoName)
                    {
                        AutoInit = false,
                        Description = options.SolutionSettings.Description,
                        LicenseTemplate = settings.LicenseExpression,
                        Private = options.SolutionSettings.GitRepoPrivate
                    };

                    var context = Manager.Instance.GitClient.Repository.Create(repository);
                    Logger.Log("Git repo created! Cloning repo...", 2);
                    _ = LibGit2Sharp.Repository.Clone(gitUrl, Path.GetDirectoryName(actualDirectory));
                    actualDirectory = Path.Combine(options.OutputDirectory, options.SolutionSettings.GitRepoName);
                    Logger.Log($"Repo cloned to '{actualDirectory}'", 2);
                    break;

                default:
                    throw new Exception($"Unknown GitRepoMode: {options.SolutionSettings.GitRepoMode}");
            }
        }
        else
        {
            Logger.Log($"Creating destination directory '{actualDirectory}' ...", 1);
            _ = Directory.CreateDirectory(actualDirectory);
            Logger.Log("Directory created!", 2);
        }

        // Step 3 - Unzip Template
        Logger.Log($"Step 3/7: Unzipping template {options.Template}...");
        IOHelpers.UnzipDirectory(actualDirectory, template.FilePath, Constants.EXCLUDED_FILES.ToList(), false);
        Logger.Log("Template unzipped!", 1);

        // Step 4 - Update the files
        Logger.Log("Step 4/7: Updating template files with solution config...");
        var replacements = GetReplacementText(options, template.Template);
        UpdateFiles(actualDirectory, replacements, template.Template.Settings.RenameOnlyFilesAndDirectories);
        Logger.Log("Files updated!", 1);

        // Step 5 - Run scripts (PYTHON TIME!)
        Logger.Log("Step 5/7: Running scripts...");
        var failedScripts = ExecuteScripts(template.Template.Settings.Scripts, actualDirectory);
        if (failedScripts.Count > 0)
        {
            var successfulScriptCount = template.Template.Settings.Scripts.Count - failedScripts.Count;

            if (successfulScriptCount > 0)
                Logger.Log(
                    $"Successfully executed {successfulScriptCount} script(s) and failed executing {failedScripts.Count} script(s)!",
                    1);
            else
                Logger.Log($"All {failedScripts.Count} scripts failed!", 1);
        }
        else
        {
            Logger.Log($"All script(s) completed! second(s)", 1);
        }

        // Step 6 - Cleanup
        Logger.Log("Step 6/7: Cleaning required files...");
        if (template.Template.Settings.CleanupFilesAndDirectories.Count == 0)
        {
            Logger.Log("Nothing to cleanup, skipping step!", 1);
        }
        else
        {
            Logger.Log("Cleaning up...", 1);
            var filesDeleted = 0;
            var directoriesDeleted = 0;
            for (var i = 0; i < template.Template.Settings.CleanupFilesAndDirectories.Count; i++)
            {
                var path = template.Template.Settings.CleanupFilesAndDirectories[i];
                if (Directory.Exists(path))
                {
                    directoriesDeleted++;
                    IOHelpers.DeleteDirectoryIfExists(path);
                }
                else
                {
                    filesDeleted++;
                    IOHelpers.DeleteFileIfExists(path);
                }
            }

            Logger.Log($"Cleaned up '{filesDeleted}' file(s) and '{directoriesDeleted}' directory(ies)!", 1);
        }

        // Step 7 - Display instructions
        Logger.Log("Step 7/7: Displaying instructions...");
        if (failedScripts.Count == 0 && template.Template.Settings.Instructions.Count == 0)
        {
            Logger.Log("No instrutions to display, skipping step!", 1);
        }
        else
        {
            var instructions = new StringBuilder();

            // append the failed scripts, if any
            if (failedScripts.Count > 0)
            {
                _ = instructions.AppendLine("The following scripts failed to execute, please execute them manually:");
                foreach (var script in failedScripts) _ = instructions.AppendLine($"- {script}");

                if (template.Template.Settings.Instructions.Count > 0)
                {
                    _ = instructions.AppendLine(" ");
                    _ = instructions.AppendLine("Instructions: ");
                }
            }

            // append the actual instructions
            foreach (var instruction in template.Template.Settings.Instructions)
                _ = instructions.AppendLine($"{instruction}");

            InstructionLogger.Log(instructions.ToString());
        }

        // DONE!
        Logger.Log("Work complete!");
        var totalTime = DateTime.Now - startTime;
        return
            $"Successfully prepared created the solution in {totalTime.TotalSeconds:0.00} second(s): {actualDirectory}";
    }

    /// <summary>
    /// Gets the unique identifier count.
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <returns></returns>
    private static int GetGuidCount(string directory)
    {
        var count = 0;

        var files = Directory.GetFiles(directory);
        foreach (var file in files)
            if (Path.GetExtension(file) == ".sln")
            {
                var contents = File.ReadAllLines(file);
                foreach (var line in contents)
                    if (line.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
                        count++;
            }

        var directories = Directory.GetDirectories(directory);
        foreach (var dir in directories) count += GetGuidCount(dir);

        return count;
    }

    /// <summary>
    /// Updates the files.
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <param name="replacements">The replacements.</param>
    /// <param name="renameOnlyFilesAndDirectories">The rename only files and directories.</param>
    private static void UpdateFiles(string directory, Dictionary<string, string> replacements,
        List<string> renameOnlyFilesAndDirectories)
    {
        var entries = Directory.GetFileSystemEntries(directory);
        foreach (var entry in entries)
        {
            // skip the .git directory
            if (entry == ".git") continue;

            var newEntryPath = UpdateText(entry, replacements);

            // Update directory naming
            if (Directory.Exists(entry))
            {
                var path = entry;
                if (entry != newEntryPath)
                {
                    Directory.Move(entry, newEntryPath);
                    path = newEntryPath;
                }

                // update inner files if and only if we should
                if (!renameOnlyFilesAndDirectories.Contains(Path.GetFileName(path)))
                    UpdateFiles(path, replacements, renameOnlyFilesAndDirectories);
            }
            // update files as needed
            else
            {
                if (entry != newEntryPath)
                {
                    if (File.Exists(newEntryPath)) File.Delete(newEntryPath);

                    File.Move(entry, newEntryPath);
                }

                if (!renameOnlyFilesAndDirectories.Contains(Path.GetFileName(newEntryPath)))
                {
                    var text = File.ReadAllText(newEntryPath);
                    text = UpdateText(text, replacements);
                    File.WriteAllText(newEntryPath, text);
                }
            }
        }
    }

    /// <summary>
    /// Updates the text.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="replacers">The replacers.</param>
    /// <returns>The updated value.</returns>
    private static string UpdateText(string value, Dictionary<string, string> replacers)
    {
        foreach (var (toBeReplaced, replacedValue) in replacers) value = value.Replace(toBeReplaced, replacedValue);
        return value;
    }

    /// <summary>
    /// Executes the provided Python scripts.
    /// </summary>
    /// <param name="scriptFiles">The script files.</param>
    /// <param name="actualDirectory">The actual directory.</param>
    /// <returns>The list of failed scripts.</returns>
    private List<string> ExecuteScripts(List<string> scriptFiles, string actualDirectory)
    {
        var failedScripts = new List<string>();

        var engine = IronPython.Hosting.Python.CreateEngine();
        for (var i = 0; i < scriptFiles.Count; i++)
        {
            var scriptFile = scriptFiles[i];
            if (Path.GetExtension(scriptFile) != ".py")
            {
                CommandLogger.Log(
                    $"Script {i + 1}/{scriptFiles.Count}: {scriptFile} is not a Python script. Skipping...", 2);
                failedScripts.Add(scriptFile);
                continue;
            }

            var scriptPath = Path.Combine(actualDirectory, scriptFile);
            CommandLogger.Log($"Running script {i + 1}/{scriptFiles.Count}: {scriptFile}...", 1);
            var scriptStartTime = DateTime.Now;
            var scriptContents = File.ReadAllText(scriptPath);
            var scope = engine.CreateScope();

            var actualWorkingDirectory = Path.GetDirectoryName(scriptPath);
            actualWorkingDirectory = actualWorkingDirectory.Replace("\\", "\\\\");
            var updatedScriptContents =
                $"import os{Environment.NewLine}os.chdir(\"{actualWorkingDirectory}\"){Environment.NewLine}{scriptContents}";
            string? errorMessage = null;
            try
            {
                engine.Execute(updatedScriptContents, scope);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            var totalScriptTime = DateTime.Now - scriptStartTime;
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                CommandLogger.Log($"Script executed successfully in {totalScriptTime.TotalSeconds:0.00} second(s)", 2);
            }
            else
            {
                CommandLogger.Log($"Script failed in {totalScriptTime.TotalSeconds:0.00} second(s)", 2);
                CommandLogger.Log($"Error Message: {errorMessage}", 2);
                failedScripts.Add(scriptFile);
            }
        }

        return failedScripts;
    }

    /// <summary>
    /// Gets the replacement text.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="template">The template.</param>
    /// <returns>The replacement text dictionary.</returns>
    private Dictionary<string, string> GetReplacementText(GenerateOptions options, Template template)
    {
        var output = new Dictionary<string, string>
        {
            // Special Text Replacement
            /* NOTE: Keep in sync with Constants.SPECIAL_TEXT
             * 0 = Current User Name
             * 1 = Current Dir
             * 2 = Solution Name
             * 3 = Project Full Name
             */
            { Constants.SPECIAL_TEXT[0], Environment.UserName },
            { Constants.SPECIAL_TEXT[1], Path.GetFileName(options.OutputDirectory) },
            { Constants.SPECIAL_TEXT[2], options.BaseName },
            { Constants.SPECIAL_TEXT[3], options.SolutionSettings.Name }
        };

        // Template-defined Replacement Text. These can be impacted by the special text replacement above, so we do
        // this after.
        foreach (var (key, value) in template.Settings.ReplacementText)
        {
            var actualKey = UpdateText(key, output);
            var actualValue = UpdateText(value, output);
            output.Add(actualKey, actualValue);
        }

        // Guids
        var guidCount = GetGuidCount(options.OutputDirectory);
        for (var i = 0; i < guidCount; i++)
        {
            var guidKey = $"GUID{i.ToString(Constants.GUID_PADDING)}";
            var guid = Guid.NewGuid().ToString();
            output.Add(guidKey, guid);
        }

        // regex tags
        var settings = (DotSlnSolutionSettings)options.SolutionSettings;
        /* NOTE: Keep in sync with DotSlnConstants.REGEX_TAGS
         * 0 = Author
         * 1 = Company
         * 2 = Tags
         * 3 = Description
         * 4 = License
         * 5 = Version
         */
        output.Add(DotSlnConstants.REGEX_TAGS[0], options.SolutionSettings.Author);
        output.Add(DotSlnConstants.REGEX_TAGS[1], settings.Company);
        output.Add(DotSlnConstants.REGEX_TAGS[2], settings.Tags);
        output.Add(DotSlnConstants.REGEX_TAGS[3], options.SolutionSettings.Description);
        output.Add(DotSlnConstants.REGEX_TAGS[4], settings.LicenseExpression);
        output.Add(DotSlnConstants.REGEX_TAGS[5], options.SolutionSettings.Version);

        return output;
    }
}