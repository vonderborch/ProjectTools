using ProjectTools.Core.Constants;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core.TemplateBuilders;

/// <summary>
///     An abstract class for templating a directory.
/// </summary>
public abstract class AbstractTemplateBuilder(string name, string description, string version, string author)
{
    /// <summary>
    ///     The name of the template builder.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     The lower-case name of the template builder.
    /// </summary>
    public string NameLowercase => this.Name.ToLowerInvariant();

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
    public List<PreparationSlug> GetPreparationSlugs(string pathToDirectoryToTemplate, PreparationTemplate template)
    {
        var slugs = new List<PreparationSlug>(template.Slugs);
        slugs = AddPreparationSlugs(SlugConstants.GetBasePreparationSlugs(), slugs);
        slugs = AddPreparationSlugs(GetPreparationSlugsForTemplate(pathToDirectoryToTemplate, template), slugs);
        return slugs;
    }

    /// <summary>
    ///     Gets a list of base slugs to parameterize the template with.
    /// </summary>
    /// <param name="pathToDirectoryToTemplate">The path to the directory being templated.</param>
    /// <param name="template">The template being used.</param>
    /// <returns>A list of slugs to request.</returns>
    public abstract List<PreparationSlug> GetPreparationSlugsForTemplate(string pathToDirectoryToTemplate,
        PreparationTemplate template);

    /// <summary>
    ///     Adds a preparation slug to the existing list of preparation slugs, if possible.
    /// </summary>
    /// <param name="newSlug">The new slug to add.</param>
    /// <param name="existingSlugs">The existing slugs.</param>
    /// <returns>The updated list of slugs.</returns>
    public List<PreparationSlug> AddPreparationSlug(PreparationSlug newSlug, List<PreparationSlug> existingSlugs)
    {
        if (existingSlugs.Any(slug => slug.SlugKey == newSlug.SlugKey))
        {
            return existingSlugs;
        }

        existingSlugs.Add(newSlug);
        return existingSlugs;
    }

    /// <summary>
    ///     Adds the preparation slugs to the existing list of preparation slugs, if possible.
    /// </summary>
    /// <param name="newSlugs">The new slugs to add.</param>
    /// <param name="existingSlugs">The existing slugs.</param>
    /// <returns>The updated list of slugs.</returns>
    public List<PreparationSlug> AddPreparationSlugs(List<PreparationSlug> newSlugs,
        List<PreparationSlug> existingSlugs)
    {
        foreach (var slug in newSlugs)
        {
            existingSlugs = AddPreparationSlug(slug, existingSlugs);
        }

        return existingSlugs;
    }

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
    ///     Checks whether a directory is valid for the template builder.
    /// </summary>
    /// <param name="pathToDirectoryToTemplate">The directory to check.</param>
    /// <returns>True if valid, False otherwise.</returns>
    public abstract bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate);
}
