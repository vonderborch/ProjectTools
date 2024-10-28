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
    private readonly List<AbstractBaseTemplater> _availableTemplaters;

    /// <summary>
    /// </summary>
    public Templater()
    {
        _availableTemplaters = new List<AbstractBaseTemplater>();
    }

    /// <summary>
    /// </summary>
    /// <param name="forceRefresh"></param>
    /// <returns></returns>
    public List<AbstractBaseTemplater> GetTemplaters(bool forceRefresh = false)
    {
        if (_availableTemplaters.Count > 0 && !forceRefresh) return _availableTemplaters;

        // Step 1 - Grab built-in templaters
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();

        var builtInTemplaters = types.Where(t => t.GetCustomAttribute<TemplaterRegistration>() != null).ToList();
        foreach (var templater in builtInTemplaters)
            _availableTemplaters.Add((AbstractBaseTemplater)Activator.CreateInstance(templater));

        // Step 2 - Grab templater plugins
        // TODO: Implement templater plugins

        return _availableTemplaters;
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
