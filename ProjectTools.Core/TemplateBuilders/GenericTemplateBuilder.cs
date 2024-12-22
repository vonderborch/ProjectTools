using ProjectTools.Core.Templates;

namespace ProjectTools.Core.TemplateBuilders;

/// <summary>
///     A generic template builder for _any_ sort of project.
/// </summary>
public class GenericTemplateBuilder()
    : AbstractTemplateBuilder("Generic", "A template builder for a generic projects.", "2.0.0", "Christian Webber")
{
    /// <summary>
    ///     Gets a list of base slugs to parameterize the template with.
    /// </summary>
    /// <param name="pathToDirectoryToTemplate">The path to the directory being templated.</param>
    /// <param name="template">The template being used.</param>
    /// <returns>A list of slugs to request.</returns>
    public override List<PreparationSlug> GetPreparationSlugsForTemplate(string pathToDirectoryToTemplate,
        PreparationTemplate template)
    {
        return new List<PreparationSlug>();
    }

    /// <summary>
    ///     Checks whether a directory is valid for the template builder.
    /// </summary>
    /// <param name="pathToDirectoryToTemplate">The directory to check.</param>
    /// <returns>True if valid, False otherwise.</returns>
    public override bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate)
    {
        return true;
    }
}
