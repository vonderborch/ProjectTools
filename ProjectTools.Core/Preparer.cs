#region

using System.Reflection;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Scripting;
using ProjectTools.Core.TemplateBuilders;
using ProjectTools.Core.Templates;

#endregion

namespace ProjectTools.Core;

/// <summary>
///     A class for templating a directory.
/// </summary>
public class Preparer
{
    /// <summary>
    ///     The available template builders.
    /// </summary>
    private readonly List<AbstractTemplateBuilder> _availableTemplateBuilders;

    /// <summary>
    ///     Constructs a new instance of <see cref="Preparer" />.
    /// </summary>
    public Preparer()
    {
        this._availableTemplateBuilders = new List<AbstractTemplateBuilder>();
    }

    /// <summary>
    ///     Generates a template from the specified directory.
    /// </summary>
    /// <param name="pathToDirectory">The path to the directory.</param>
    /// <param name="outputDirectory">The output directory.</param>
    /// <param name="skipCleaning">Whether to skip cleaning or not.</param>
    /// <param name="forceOverride">Whether to delete any existing template or not.</param>
    /// <param name="template">The template info.</param>
    /// <param name="coreLogger">The core logger.</param>
    /// <returns>The results of the template prep process.</returns>
    public string GenerateTemplate(string pathToDirectory, string outputDirectory, bool skipCleaning,
        bool forceOverride, PreparationTemplate template, Logger coreLogger)
    {
        var outputTempDirectory = Path.Combine(outputDirectory, $"{template.SafeName}_TEMPLATEGEN");
        var outputFile =
            Path.Combine(outputDirectory, $"{template.SafeName}.{TemplateConstants.TemplateFileExtension}");

        // Step 1 - Cleanup directories/files
        coreLogger.Log("Step 1/7 - Cleaning existing directories and files...");
        if (!IOHelpers.CleanDirectory(outputTempDirectory, forceOverride))
        {
            return "Directory already exists and force override is not enabled.";
        }

        if (!IOHelpers.CleanFile(outputFile, forceOverride))
        {
            return "Output file already exists and force override is not enabled.";
        }

        // Step 2 - Copy the directory we are trying to template to the output directory
        coreLogger.Log("Step 2/7 - Copying directory to temp directory...");
        var excludedDirectories = template.PrepareExcludedPaths.Where(x => x.EndsWith("/")).ToList();
        var excludedFiles = template.PrepareExcludedPaths.Where(x => !x.EndsWith("/")).ToList();
        IOHelpers.CopyDirectory(pathToDirectory, outputTempDirectory, excludedDirectories, excludedFiles);

        // Step 3 - Run all the prepare scripts
        coreLogger.Log("Step 3/7 - Executing preparation scripts...");
        if (template.PrepareScripts.Count > 0)
        {
            foreach (var scriptPath in template.PrepareScripts)
            {
                coreLogger.Log($"Executing Script {scriptPath}...", 2);
                var (success, exception) =
                    PythonManager.Manager.ExecuteScript(outputTempDirectory, scriptPath, coreLogger, 2);
                if (success)
                {
                    coreLogger.Log("Script Executed!", 4);
                }
                else
                {
                    coreLogger.Log($"Error running script {scriptPath}: {exception?.Message}", 4);
                    throw exception;
                }
            }
        }
        else
        {
            coreLogger.Log("No preparation scripts to execute!", 2);
        }

        // Step 4 - Go through the slugs and replace all instances of the search terms with the slug key
        coreLogger.Log("Step 4/7 - Templatizing directory...");
        UpdateFileSystemEntries(outputTempDirectory, outputTempDirectory, template, false);

        // Step 5 - Create the template info file
        coreLogger.Log("Step 5/7 - Creating template settings file...");
        var outputTemplateInfoFile = Path.Combine(outputTempDirectory, TemplateConstants.TemplateSettingsFileName);
        JsonHelpers.SerializeToFile(outputTemplateInfoFile, template.ToTemplate());

        // Step 6 - Convert the template directory to a zip file
        coreLogger.Log("Step 6/7 - Archiving directory...");
        IOHelpers.ArchiveDirectory(outputTempDirectory, outputFile, skipCleaning);

        // Step 7 - DONE
        coreLogger.Log("Step 7/7 - Cleaning directory...");
        if (!skipCleaning)
        {
            IOHelpers.CleanDirectory(outputTempDirectory, true);
        }

        coreLogger.Log("Success!");
        return "";
    }

    /// <summary>
    ///     Gets the template builder for the specified option and directory.
    /// </summary>
    /// <param name="option">The template builder selected, or auto.</param>
    /// <param name="directory">The directory we want to prepare as a template.</param>
    /// <returns>The template builder.</returns>
    /// <exception cref="Exception">Raised if we failed to find a valid template builder.</exception>
    public AbstractTemplateBuilder GetTemplateBuilderForOption(string option, string directory)
    {
        var templateBuilders = GetTemplateBuilders();
        AbstractTemplateBuilder? templateBuilderForPrep = null;
        // If template builder is auto, try to detect the correct one
        if (option == "auto")
        {
            foreach (var templateBuilder in templateBuilders)
            {
                if (templateBuilder.IsValidDirectoryForBuilder(directory))
                {
                    templateBuilderForPrep = templateBuilder;
                    break;
                }
            }
        }
        // Otherwise, try to find the template builder by name...
        else
        {
            var templateBuilder = templateBuilders.FirstOrDefault(x => x.NameLowercase == option);
            if (templateBuilder != null)
            {
                if (templateBuilder.IsValidDirectoryForBuilder(directory))
                {
                    templateBuilderForPrep = templateBuilder;
                }
            }
        }

        // Raise an exception if we couldn't find a valid template builder, otherwise return the template builder
        if (templateBuilderForPrep == null)
        {
            throw new Exception("Could not detect valid template builder for directory!");
        }

        return templateBuilderForPrep;
    }

    /// <summary>
    ///     Gets all template builders.
    /// </summary>
    /// <param name="forceRefresh">True to force refresh, False otherwise.</param>
    /// <returns>All available template builders.</returns>
    public List<AbstractTemplateBuilder> GetTemplateBuilders(bool forceRefresh = false)
    {
        if (this._availableTemplateBuilders.Count > 0 && !forceRefresh)
        {
            return this._availableTemplateBuilders;
        }

        // Step 1 - Grab built-in template builders
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();

        var builtInTemplateBuilders =
            types.Where(t => t is { IsClass: true, IsAbstract: false } && t.BaseType == typeof(AbstractTemplateBuilder))
                .ToList();
        foreach (var templater in builtInTemplateBuilders)
        {
            this._availableTemplateBuilders.Add((AbstractTemplateBuilder)Activator.CreateInstance(templater));
        }

        // Step 2 - Grab template builders plugins
        // TODO: Implement template builders plugins

        return this._availableTemplateBuilders;
    }

    /// <summary>
    ///     Updates the directory files per the template and replacement text.
    /// </summary>
    /// <param name="directory">The directory to update.</param>
    /// <param name="rootDirectory">The root output directory.</param>
    /// <param name="template">The template.</param>
    /// <param name="isRenameOnlyPath">Whether we're already in a rename-only path or not.</param>
    private void UpdateFileSystemEntries(string directory, string rootDirectory, PreparationTemplate template,
        bool isRenameOnlyPath)
    {
        var fileSystemEntries = Directory.GetFileSystemEntries(directory);
        foreach (var entry in fileSystemEntries)
        {
            var baseEntryName = Path.GetFileName(entry);
            var entryActualName = UpdateText(baseEntryName, template.ReplacementText);

            var actualPath = Path.Combine(Path.GetDirectoryName(entry) ?? string.Empty, entryActualName);
            var entryIsRenameOnlyPath =
                PathHelpers.PathIsInList(actualPath, rootDirectory, template.RenameOnlyPaths, true, true);

            var isRenameOnlyPathForEntry = isRenameOnlyPath || entryIsRenameOnlyPath;

            // Handle directories
            if (Directory.Exists(entry))
            {
                // if the base name doesn't match the actual name, we need to rename the directory...
                if (baseEntryName != entryActualName)
                {
                    if (Directory.Exists(actualPath))
                    {
                        throw new Exception($"Path `{actualPath}` already exists for renamed path `{entry}`!");
                    }

                    Directory.Move(entry, actualPath);
                }

                // then we update the sub-directory!
                UpdateFileSystemEntries(actualPath, rootDirectory, template, isRenameOnlyPathForEntry);
            }
            // Handle files
            else
            {
                // if the base name doesn't match the actual name, we need to rename the file...
                if (baseEntryName != entryActualName)
                {
                    if (File.Exists(actualPath))
                    {
                        throw new Exception($"Path `{actualPath}` already exists for renamed path `{entry}`!");
                    }

                    File.Move(entry, actualPath);
                }

                if (!isRenameOnlyPathForEntry)
                {
                    var inputLines = File.ReadAllLines(actualPath);
                    List<string> outputLines = new();
                    foreach (var line in inputLines)
                    {
                        var updatedLine = UpdateText(line, template.ReplacementText);
                        outputLines.Add(updatedLine);
                    }

                    File.WriteAllLines(actualPath, outputLines);
                }
            }
        }
    }

    /// <summary>
    ///     Updates the specified text with all instances of the replacementText dict.
    /// </summary>
    /// <param name="text">The base text.</param>
    /// <param name="replacementText">Keys to search for and replace with the specified values.</param>
    /// <returns>The updated text.</returns>
    private string UpdateText(string text, Dictionary<string, string> replacementText)
    {
        foreach (var (key, value) in replacementText)
        {
            text = text.Replace(key, value);
        }

        return text;
    }
}
