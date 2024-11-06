namespace ProjectTools.Core.Templates;

/// <summary>
/// A slug used when creating a new template.
/// </summary>
public class PreparationSlug
{
    /// <summary>
    /// The allowed values for the slug.
    /// </summary>
    public List<object?> AllowedValues = [];

    /// <summary>
    /// The default value for the slug.
    /// </summary>
    public object? DefaultValue = null;

    /// <summary>
    /// The disallowed values for the slug.
    /// </summary>
    public List<object?> DisallowedValues = [];

    /// <summary>
    /// The display name of the slug.
    /// </summary>
    public required string DisplayName;

    /// <summary>
    ///     True if we need to ask about this preparation slug _at all_, false otherwise.
    /// </summary>
    public bool RequiresAnyInput = true;

    /// <summary>
    /// True if the slug requires user input, false otherwise.
    /// </summary>
    public bool RequiresUserInput = false;

    /// <summary>
    /// The strings we search for to replace with the SlugKey.
    /// </summary>
    public required List<string> SearchStrings;

    /// <summary>
    /// The key used to identify where to replace the slug with the current value for the slug.
    /// </summary>
    public required string SlugKey;

    /// <summary>
    /// The type of the slug.
    /// </summary>
    public required SlugType Type;

    /// <summary>
    ///     Converts a PreparationSlug to a Slug.
    /// </summary>
    /// <returns>The Slug.</returns>
    public Slug ToSlug()
    {
        return new Slug
        {
            SlugKey = SlugKey,
            DisplayName = DisplayName,
            Type = Type,
            DefaultValue = DefaultValue,
            AllowedValues = AllowedValues,
            DisallowedValues = DisallowedValues,
            RequiresUserInput = RequiresUserInput
        };
    }
}
