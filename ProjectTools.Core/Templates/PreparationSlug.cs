namespace ProjectTools.Core.Templates;

/// <summary>
///     A slug used when creating a new template.
/// </summary>
public class PreparationSlug : Slug
{
    /// <summary>
    ///     True if we need to ask about this preparation slug _at all_, false otherwise.
    /// </summary>
    public bool RequiresAnyInput = true;

    /// <summary>
    ///     The strings we search for to replace with the SlugKey.
    /// </summary>
    public required List<string> SearchStrings;

    /// <summary>
    ///     Converts a PreparationSlug to a Slug.
    /// </summary>
    /// <returns>The Slug.</returns>
    public Slug ToSlug()
    {
        return new Slug
        {
            SlugKey = this.SlugKey,
            DisplayName = this.DisplayName,
            Type = this.Type,
            DefaultValue = this.DefaultValue,
            AllowedValues = this.AllowedValues,
            DisallowedValues = this.DisallowedValues,
            RequiresUserInput = this.RequiresUserInput
        };
    }
}