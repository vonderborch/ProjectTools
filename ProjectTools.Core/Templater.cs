using System.Reflection;
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

    /// <summary>
    /// </summary>
    /// <param name="pathToDirectoryToTemplate"></param>
    /// <param name="newTemplate"></param>
    /// <returns></returns>
    public string GenerateTemplate(string pathToDirectoryToTemplate, Template newTemplate)
    {
        return string.Empty;
    }
}
