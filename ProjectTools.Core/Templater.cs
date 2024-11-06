using System.Reflection;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templaters;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core;

/// <summary>
///     An class for templating a directory.
/// </summary>
public class Templater
{
    /// <summary>
    /// </summary>
    private readonly List<AbstractTemplateBuilder> _availableTemplateBuilders;

    /// <summary>
    /// </summary>
    public Templater()
    {
        _availableTemplateBuilders = new List<AbstractTemplateBuilder>();
    }

    /// <summary>
    /// </summary>
    /// <param name="forceRefresh"></param>
    /// <returns></returns>
    public List<AbstractTemplateBuilder> GetTemplateBuilders(bool forceRefresh = false)
    {
        if (_availableTemplateBuilders.Count > 0 && !forceRefresh) return _availableTemplateBuilders;

        // Step 1 - Grab built-in template builders
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();

        var builtInTemplateBuilders =
            types.Where(t => t.IsClass && !t.IsAbstract && t.BaseType == typeof(AbstractTemplateBuilder)).ToList();
        foreach (var templater in builtInTemplateBuilders)
            _availableTemplateBuilders.Add((AbstractTemplateBuilder)Activator.CreateInstance(templater));

        // Step 2 - Grab template builders plugins
        // TODO: Implement template builders plugins

        return _availableTemplateBuilders;
    }

    public string GenerateTemplate(string pathToDirectory, string outputDirectory, bool skipCleaning,
        bool forceOverride, List<PreparationSlug> slugs, Template template)
    {
        var outputTempDirectory = Path.Combine(outputDirectory, template.SafeName);
        var outputFile = Path.Combine(outputDirectory, $"{template.SafeName}.{PathConstants.TemplateFileExtension}");

        // Step 1 - Copy the directory to the output directory
        if (!PathHelpers.CleanDirectory(outputTempDirectory))
            return "Directory already exists and force override is not enabled.";

        if (!PathHelpers.CleanFile(outputFile)) return "Directory already exists and force override is not enabled.";


        if (Directory.Exists(outputTempDirectory))
        {
            if (forceOverride)
                File.Delete(outputFile);
            else
                return "File already exists and force override is not enabled.";
        }

        // Step 2 - Go through the slugs and replace all instances of the search terms with the slug key

        // Step 3 - PrepSlugs -> Slugs and attach to the template
        var finalSlugs = slugs.Select(slug => slug.ToSlug()).ToList();
        template.Slugs = finalSlugs;

        // Step 4 - Create the template info file


        // Step 5 - Convert the template directory to a zip file

        // Step 6 - DONE
        return "Success!";
    }
}
