using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

/// <summary>
///     An abstract class for templating a directory.
/// </summary>
public abstract class AbstractTemplateBuilder(string name, string description, string version, string author)
{
    public string Name { get; private set; } = name;

    public string NameLowercase => Name.ToLowerInvariant();

    public string Description { get; private set; } = description;

    public string Version { get; private set; } = version;

    public string Author { get; private set; } = author;

    /// <summary>
    ///     Gets a list of base slugs to parameterize the template with.
    /// </summary>
    /// <param name="pathToDirectoryToTemplate">The path to the directory being templated.</param>
    /// <returns>A list of slugs to request.</returns>
    public abstract List<PreperationSlug> GetBaseSlugs(string pathToDirectoryToTemplate);

    public virtual void ReplaceSearchStringsWithSlugs(List<PreperationSlug> slugs, string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }

    public abstract bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate);
}
