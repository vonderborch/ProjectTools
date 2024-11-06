using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

/// <summary>
///     An abstract class for templating a directory.
/// </summary>
public abstract class AbstractTemplateBuilder(string name, string description, string version, string author)
{
    /// <summary>
    ///     The name of the template builder.
    /// </summary>
    public string Name { get; private set; } = name;

    /// <summary>
    ///     The lower-case name of the template builder.
    /// </summary>
    public string NameLowercase => Name.ToLowerInvariant();

    /// <summary>
    ///     The description of the template builder.
    /// </summary>
    public string Description { get; private set; } = description;

    /// <summary>
    ///     The version of the template builder.
    /// </summary>
    public string Version { get; private set; } = version;

    /// <summary>
    ///     The author of the template builder.
    /// </summary>
    public string Author { get; private set; } = author;

    /// <summary>
    ///     Gets a list of base slugs to parameterize the template with.
    /// </summary>
    /// <param name="pathToDirectoryToTemplate">The path to the directory being templated.</param>
    /// <param name="template">The template being used.</param>
    /// <returns>A list of slugs to request.</returns>
    public abstract List<PreparationSlug> GetPreparationSlugs(string pathToDirectoryToTemplate, Template template);

    /// <summary>
    ///     Goes through the directory and prepares it for templating.
    /// </summary>
    /// <param name="slugs">The slugs.</param>
    /// <param name="template">The template.</param>
    /// <param name="pathToDirectoryToTemplate">The path to the directory.</param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual void PrepareDirectory(List<PreparationSlug> slugs, Template template,
        string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Converts the preparation slugs to slugs.
    /// </summary>
    /// <param name="slugs">The preparation slugs to convert.</param>
    /// <returns>The slugs.</returns>
    public List<Slug> GetSlugsFromPreparationSlugs(List<PreparationSlug> slugs)
    {
        var output = slugs.Select(slug => slug.ToSlug()).ToList();
        return output;
    }

    /// <summary>
    ///     Checks whether a directory is valid for the template builder.
    /// </summary>
    /// <param name="pathToDirectoryToTemplate">The directory to check.</param>
    /// <returns>True if valid, False otherwise.</returns>
    public abstract bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate);
}
