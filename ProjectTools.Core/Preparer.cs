using System.Reflection;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.TemplateBuilders;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core;

/// <summary>
///     A class for templating a directory.
/// </summary>
public class Preparer
{
    /// <summary>
    /// </summary>
    private readonly List<AbstractTemplateBuilder> _availableTemplateBuilders;

    /// <summary>
    /// </summary>
    public Preparer()
    {
        this._availableTemplateBuilders = new List<AbstractTemplateBuilder>();
    }

    /// <summary>
    /// </summary>
    /// <param name="forceRefresh"></param>
    /// <returns></returns>
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

    public string GenerateTemplate(string pathToDirectory, string outputDirectory, bool skipCleaning,
        bool forceOverride, PreparationTemplate template, Logger coreLogger, Logger instructionLogger)
    {
        var outputTempDirectory = Path.Combine(outputDirectory, template.SafeName);
        var outputFile =
            Path.Combine(outputDirectory, $"{template.SafeName}.{TemplateConstants.TemplateFileExtension}");

        // Step 1 - Cleanup directories/files
        coreLogger.Log("Step 1/8 - Cleaning existing directories and files...");
        if (!IOHelpers.CleanDirectory(outputTempDirectory, forceOverride))
        {
            return "Directory already exists and force override is not enabled.";
        }

        if (!IOHelpers.CleanFile(outputFile, forceOverride))
        {
            return "Output file already exists and force override is not enabled.";
        }

        // Step 2 - Copy the directory we are trying to template to the output directory
        coreLogger.Log("Step 2/8 - Copying directory to temp directory...");
        IOHelpers.CopyDirectory(pathToDirectory, outputTempDirectory, template.PrepareExcludedPaths);

        // Step 3 - Go through the slugs and replace all instances of the search terms with the slug key
        coreLogger.Log("Step 3/8 - Templatizing directory...");
        UpdateDirectoryFiles(outputTempDirectory, outputTempDirectory, template, false);

        // Step 4 - Create the template info file
        coreLogger.Log("Step 4/8 - Creating template settings file...");
        var outputTemplateInfoFile = Path.Combine(outputTempDirectory, TemplateConstants.TemplateSettingsFileName);
        JsonHelpers.SerializeToFile(outputTemplateInfoFile, template.ToTemplate());

        // Step 5 - Convert the template directory to a zip file
        coreLogger.Log("Step 5/8 - Archiving directory...");
        IOHelpers.ArchiveDirectory(outputTempDirectory, outputFile, skipCleaning);

        // TODO - Step 6 - Try to execute scripts!
        coreLogger.Log("Step 6/8 - Executing scripts...");

        // TODO - Step 7 - List instructions!
        coreLogger.Log("Step 7/8 - Listing instructions...");

        // Step 8 - DONE
        coreLogger.Log("Step 8/8 - Cleaning directory...");
        if (!skipCleaning)
        {
            IOHelpers.CleanDirectory(outputTempDirectory, true);
        }

        coreLogger.Log("Success!");
        return "";
    }


    /// <summary>
    ///     Updates the directory files per the template and replacement text.
    /// </summary>
    /// <param name="directory">The directory to update.</param>
    /// <param name="rootOutputDirectory">The root output directory.</param>
    /// <param name="template">The template.</param>
    /// <param name="isRenameOnlyPath">Whether we're already in a rename-only path or not.</param>
    private void UpdateDirectoryFiles(string directory, string rootOutputDirectory, PreparationTemplate template,
        bool isRenameOnlyPath)
    {
        // Go through sub-directories and update them...
        var directories = Directory.GetDirectories(directory);
        foreach (var subdirectory in directories)
        {
            // update the files in the sub-directory...
            UpdateDirectoryFiles(subdirectory, rootOutputDirectory, template,
                IsRenameOnlyPath(subdirectory, rootOutputDirectory, template, isRenameOnlyPath));

            // make sure to rename the sub-directory...
            var actualDirectoryName = UpdateText(Path.GetFileName(directory), template.ReplacementText);
            var baseDirectoryName = Path.GetFileName(directory);
            if (actualDirectoryName != baseDirectoryName)
            {
                var actualDirectory = Path.Combine(Path.GetDirectoryName(subdirectory), actualDirectoryName);
                IOHelpers.CopyDirectory(subdirectory, actualDirectory, []);
                Directory.Delete(subdirectory, true);
            }
        }

        // update all files in the current directory...
        var files = Directory.GetFiles(directory)
            .Where(f => Path.GetFileName(f) != TemplateConstants.TemplateSettingsFileName);
        foreach (var file in files)
        {
            UpdateFile(file, rootOutputDirectory, template,
                IsRenameOnlyPath(file, rootOutputDirectory, template, isRenameOnlyPath));
        }
    }

    /// <summary>
    ///     Returns whether the path is a rename-only path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="rootOutputDirectory">The root output directory.</param>
    /// <param name="template">The template.</param>
    /// <param name="isRenameOnlyPath">Whether we're already in a rename-only path or not.</param>
    /// <returns>True for rename-only paths, False otherwise.</returns>
    private bool IsRenameOnlyPath(string path, string rootOutputDirectory, PreparationTemplate template,
        bool isRenameOnlyPath)
    {
        if (isRenameOnlyPath)
        {
            return true;
        }

        var relativePath = Path.GetRelativePath(rootOutputDirectory, path);
        return template.RenameOnlyPaths.Contains(relativePath);
    }

    /// <summary>
    ///     Updates the specified file by replacing all instances of the text in the replacementText dict.
    /// </summary>
    /// <param name="file">The file to update.</param>
    /// <param name="rootOutputDirectory">The root output directory.</param>
    /// <param name="template">The template.</param>
    /// <param name="isRenameOnlyPath">Whether we're already in a rename-only path or not.</param>
    private void UpdateFile(string file, string rootOutputDirectory, PreparationTemplate template,
        bool isRenameOnlyPath)
    {
        // Step 1 - Get a temp file name...
        string tempFile;
        var fileDirectory = Path.GetDirectoryName(file);
        do
        {
            tempFile = Path.Combine(fileDirectory, Path.GetRandomFileName());
        } while (File.Exists(tempFile));

        // Step 2 - Determine the output file name
        var relativeFilePath = Path.GetRelativePath(rootOutputDirectory, file);
        var outputFileName = Path.GetFileName(relativeFilePath);
        var outputFile = UpdateText(outputFileName, template.ReplacementText);
        outputFile = Path.Combine(rootOutputDirectory, Path.GetDirectoryName(relativeFilePath), outputFile);

        // Step 3 - Is the file one we need to update contents of, or not?
        if (isRenameOnlyPath)
        {
            // Step 3A - This is a file we want to rename _only_
            if (file != outputFile)
            {
                File.Copy(file, outputFile);
                File.Delete(file);
            }
        }
        else
        {
            // Step 3B - This is a file we want to update the contents of
            // Step 3B1 - Copy the file to the temp file, and delete the original file
            File.Copy(file, tempFile);
            File.Delete(file);

            // Step 3B2 - Open the temp file for reading and the original file for writing
            using (var output = File.Create(outputFile))
            {
                using var writer = new StreamWriter(output);
                using var input = File.OpenRead(tempFile);
                using var reader = new StreamReader(input);

                // Step 3B3 - Go through line-by-line and replace all search text with the provided slug key
                var line = reader.ReadLine();
                while (line != null)
                {
                    line = UpdateText(line, template.ReplacementText);
                    writer.WriteLine(line);
                    line = reader.ReadLine();
                }
            }

            // Step 3B4 - Delete the temp file now that we're done with it!
            File.Delete(tempFile);
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
