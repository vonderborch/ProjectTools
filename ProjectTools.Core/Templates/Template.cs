#region

using System.Diagnostics;
using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Scripting;
using ProjectTools.Core.Settings;

#endregion

namespace ProjectTools.Core.Templates;

/// <summary>
///     A template, used to create or extend projects.
/// </summary>
[DebuggerDisplay("{Name}")]
public class Template : AbstractTemplate
{
    /// <summary>
    ///     A cache of the slug's current values.
    /// </summary>
    private Dictionary<string, string>? _slugValuesCache;

    /// <summary>
    ///     Information on slugs for this template, used to replace instances of the slug with the value of the slug.
    /// </summary>
    public List<Slug> Slugs = [];

    /// <summary>
    ///     Generates a project using the template.
    /// </summary>
    /// <param name="parentOutputDirectory">The output directory.</param>
    /// <param name="name">The name of the project.</param>
    /// <param name="pathToTemplate">The path to the template.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="instructionLogger">The instruction logger.</param>
    /// <param name="commandLogger">The command logger.</param>
    /// <param name="overrideExisting">True to override the existing directory.</param>
    /// <returns>The results.</returns>
    public async Task<string> AsyncGenerateProject(string parentOutputDirectory, string name, string pathToTemplate,
        Logger logger,
        Logger instructionLogger,
        Logger commandLogger, bool overrideExisting = false)
    {
        return GenerateProject(parentOutputDirectory, name, pathToTemplate, logger, instructionLogger, commandLogger,
            overrideExisting);
    }

    /// <summary>
    ///     Generates a project using the template.
    /// </summary>
    /// <param name="parentOutputDirectory">The output directory.</param>
    /// <param name="name">The name of the project.</param>
    /// <param name="pathToTemplate">The path to the template.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="instructionLogger">The instruction logger.</param>
    /// <param name="commandLogger">The command logger.</param>
    /// <param name="overrideExisting">True to override the existing directory.</param>
    /// <returns>The results.</returns>
    public string GenerateProject(string parentOutputDirectory, string name, string pathToTemplate,
        Logger logger,
        Logger instructionLogger,
        Logger commandLogger, bool overrideExisting = false)
    {
        var specialValueHandler = new SpecialValueHandler(parentOutputDirectory, name, this);
        var startTime = DateTime.Now;
        var appSettings = AbstractSettings.LoadOrThrow();

        var outputDirectory = Path.Combine(parentOutputDirectory, name);
        if (!IOHelpers.CleanDirectory(outputDirectory, overrideExisting))
        {
            return
                $"Directory already exists and override is not set to true. Skipping generation of project {name}.";
        }

        // Step 1 - Create destination directory
        logger.Log("Step 1/6: Creating directory...");
        Directory.CreateDirectory(outputDirectory);
        logger.Log("Directory created.", 2);

        // Step 2 - Unzip the template
        logger.Log($"Step 2/6: Unzipping template {this.SafeName}...");
        IOHelpers.UnzipDirectory(outputDirectory, pathToTemplate, TemplateConstants.GeneratedProjectExcludedFileNames,
            false);
        logger.Log("Template unzipped!", 1);

        // Step 3 - Update the files
        logger.Log("Step 3/6: Updating file system objects...");
        UpdateFiles(outputDirectory, outputDirectory, specialValueHandler);
        logger.Log("Files updated!", 2);

        // Step 4 - Run scripts (C# SCRIPT TIME!?)
        logger.Log("Step 4/6: Running scripts...");
        List<string> instructions = new();
        if (this.PythonScriptPaths.Count > 0)
        {
            foreach (var scriptPath in this.PythonScriptPaths.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                commandLogger.Log($"Executing Script {scriptPath}...", 2);
                var (success, exception) =
                    PythonManager.Manager.ExecuteScript(outputDirectory, scriptPath, commandLogger, 2);
                if (success)
                {
                    commandLogger.Log("Script Executed!", 4);
                }
                else
                {
                    commandLogger.Log($"Error running script {scriptPath}: {exception?.Message}", 4);
                    instructions.Add($"Run script: {Path.Combine(outputDirectory, scriptPath)}");
                }
            }
        }
        else
        {
            logger.Log("No scripts to execute!", 2);
        }

        // Step 5 - List instructions
        logger.Log("Step 5/6: Listing instructions...");
        instructions = instructions.CombineLists(this.Instructions);
        if (instructions.Count > 0)
        {
            foreach (var instruction in instructions.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                instructionLogger.Log(instruction);
            }
        }
        else
        {
            logger.Log("No instructions to display!", 2);
        }

        // Step 6 - Cleanup
        logger.Log("Step 6/6: Cleaning template...");
        if (this.PathsToRemove.Count > 0)
        {
            foreach (var path in this.PathsToRemove)
            {
                var actualPath = Path.Combine(outputDirectory, path);
                if (Directory.Exists(actualPath))
                {
                    IOHelpers.DeleteDirectoryIfExists(actualPath);
                }
                else if (File.Exists(actualPath))
                {
                    IOHelpers.DeleteFileIfExists(actualPath);
                }
            }
        }
        logger.Log("Cleaning complete!", 2);

        // DONE!
        logger.Log("Work complete!");
        var totalTime = DateTime.Now - startTime;
        return
            $"Successfully prepared created the solution in {totalTime.TotalSeconds:0.00} second(s): {outputDirectory}";
    }

    /// <summary>
    ///     Gets the slug key -> value mapping dictionary.
    /// </summary>
    /// <returns>The mapping dictionary.</returns>
    private Dictionary<string, string> SlugsToValues()
    {
        if (this._slugValuesCache == null)
        {
            this._slugValuesCache =
                this.Slugs.ToDictionary(s => s.ActualSlugKey, s => s.CurrentValue);
        }

        return this._slugValuesCache;
    }

    /// <summary>
    ///     Updates the files in the provided directory.
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <param name="rootDirectory">The root directory.</param>
    /// <param name="specialValueHandler">The special value handler.</param>
    /// <param name="isRenameOnlyPath">If the path is a rename-only path or not.</param>
    private void UpdateFiles(string directory, string rootDirectory, SpecialValueHandler specialValueHandler, bool isRenameOnlyPath = false)
    {
        var entries = Directory.GetFileSystemEntries(directory);

        foreach (var entry in entries)
        {
            if (TemplateConstants.ExcludedDirectoryNamesDuringProjectGeneration.Contains(entry))
            {
                continue;
            }

            var isRenameOnly = PathHelpers.PathIsInList(entry, rootDirectory, this.RenameOnlyPaths, true, true);
            var isRenameOnlyPathForEntry = isRenameOnlyPath || isRenameOnly;
            var newEntryPath = UpdateText(entry, specialValueHandler);

            // Update directory naming...
            if (Directory.Exists(entry))
            {
                var path = entry;
                if (entry != newEntryPath)
                {
                    Directory.Move(entry, newEntryPath);
                    path = newEntryPath;
                }

                // update inner files if and only if we should
                if (!isRenameOnly)
                {
                    UpdateFiles(path, rootDirectory, specialValueHandler, isRenameOnlyPathForEntry);
                }
            }
            // Otherwise, update the files as needed
            else
            {
                if (entry != newEntryPath)
                {
                    if (File.Exists(newEntryPath))
                    {
                        File.Delete(newEntryPath);
                    }

                    File.Move(entry, newEntryPath);
                }

                if (!isRenameOnlyPathForEntry)
                {
                    var text = File.ReadAllText(newEntryPath);
                    text = UpdateText(text, specialValueHandler);
                    File.WriteAllText(newEntryPath, text);
                }
            }
        }
    }

    /// <summary>
    ///     Updates the provided test with the slug values.
    /// </summary>
    /// <param name="value">The text.</param>
    /// <param name="specialValueHandler">The special value handler.</param>
    /// <returns>The updated text.</returns>
    private string UpdateText(string value, SpecialValueHandler specialValueHandler)
    {
        var originalValue = value;
        foreach (var (toBeReplaced, replacedValue) in SlugsToValues())
        {
            value = value.Replace(toBeReplaced, replacedValue);
        }

        foreach (var special in specialValueHandler.Values)
        {
            value = value.Replace(special.Key, special.Value);
        }

        return value;
    }
}
