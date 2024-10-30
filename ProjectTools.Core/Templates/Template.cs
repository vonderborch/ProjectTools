namespace ProjectTools.Core.Templates;

/// <summary>
/// A template, used to create or extend projects.
/// </summary>
public class Template
{
    /// <summary>
    /// The author of the template.
    /// </summary>
    public string Author = string.Empty;

    /// <summary>
    /// The description of the template.
    /// </summary>
    public string Description = string.Empty;

    /// <summary>
    /// The name of the template.
    /// </summary>
    public string Name = string.Empty;

    /// <summary>
    /// Paths to delete once a project is created or extended using this template.
    /// </summary>
    public List<string> PathsToRemove = [];

    /// <summary>
    /// Paths to Python scripts to execute.
    /// </summary>
    public List<string> PythonScriptPaths = [];

    /// <summary>
    /// Paths within the template that should only be renamed as needed, not having their contents updated.
    /// </summary>
    public List<string> RenameOnlyPaths = [];

    /// <summary>
    /// Information on slugs for this template, used to replace instances of the slug with the value of the slug.
    /// </summary>
    public List<Slug> Slugs = [];

    /// <summary>
    /// The name of the template builder to use.
    /// </summary>
    public string TemplateBuilder = string.Empty;

    /// <summary>
    /// The current version of the template.
    /// </summary>
    public string Version = string.Empty;
}
