using System.Diagnostics;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Settings;

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

    public string GenerateProject(string parentOutputDirectory, string name, string pathToTemplate,
        Logger logger,
        Logger instructionLogger,
        Logger commandLogger, bool overrideExisting = false)
    {
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
        UpdateFiles(outputDirectory, outputDirectory);
        logger.Log("Files updated!", 2);

        // Step 4 - Run scripts (PYTHON TIME!?)
        logger.Log("Step 4/6: Running scripts...");
        List<string> instructions = new();
        if (this.PythonScriptPaths.Count > 0)
        {
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
        }
        else
        {
            logger.Log("No instructions to display!", 2);
        }

        // Step 6 - Cleanup
        logger.Log("Step 6/6: Cleaning things up...");

        // DONE!
        logger.Log("Work complete!");
        var totalTime = DateTime.Now - startTime;
        return
            $"Successfully prepared created the solution in {totalTime.TotalSeconds:0.00} second(s): {outputDirectory}";
    }

    private void UpdateFiles(string directory, string rootDirectory, bool isRenameOnlyPath = false)
    {
        var entries = Directory.GetFileSystemEntries(directory);

        foreach (var entry in entries)
        {
            if (TemplateConstants.ExcludedDirectoryNamesDuringProjectGeneration.Contains(entry))
            {
                continue;
            }

            var isRenameOnly = PathHelpers.PathIsInList(entry, rootDirectory, this.RenameOnlyPaths, true, true);
            isRenameOnlyPath = isRenameOnlyPath || isRenameOnly;
            var newEntryPath = UpdateText(entry);

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
                    UpdateFiles(path, rootDirectory, isRenameOnlyPath);
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

                if (!isRenameOnlyPath)
                {
                    var text = File.ReadAllText(newEntryPath);
                    text = UpdateText(text);
                    File.WriteAllText(newEntryPath, text);
                }
            }
        }
    }

    /// <summary>
    ///     Updates the provided test with the slug values.
    /// </summary>
    /// <param name="value">The text.</param>
    /// <returns>The updated text.</returns>
    private string UpdateText(string value)
    {
        foreach (var (toBeReplaced, replacedValue) in SlugsToValues())
        {
            value = value.Replace(toBeReplaced, replacedValue);
        }

        return value;
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
                this.Slugs.ToDictionary(s => s.ActualSlugKey, s => s.Type.ObjectToString(s.CurrentValue));
        }

        return this._slugValuesCache;
    }
}
