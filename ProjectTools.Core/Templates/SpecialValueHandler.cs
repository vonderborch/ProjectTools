namespace ProjectTools.Core.Templates;

/// <summary>
///     A handler for special keyword values with slugs.
/// </summary>
public class SpecialValueHandler
{
    /// <summary>
    ///     The special values dictionary.
    /// </summary>
    private readonly Dictionary<SlugType, Dictionary<string, string>> _specialValues = new();

    /// <summary>
    ///     Creates a new instance of the <see cref="SpecialValueHandler" /> class.
    /// </summary>
    /// <param name="parentDirectory">The parent directory.</param>
    /// <param name="projectName">The project name.</param>
    /// <param name="template">The template.</param>
    public SpecialValueHandler(string parentDirectory, string projectName, Template? template)
    {
        foreach (var slugType in Enum.GetValues<SlugType>())
        {
            this._specialValues[slugType] = new Dictionary<string, string>();
        }

        //// String Special Values
        // Add special values for project name
        this._specialValues[SlugType.String]["[[ProjectName]]"] = projectName;

        // Add special values for parent directories
        this._specialValues[SlugType.String]["[[ParentDirectory]]"] = parentDirectory;
        this._specialValues[SlugType.String]["[[ParentDirectoryName]]"] = Path.GetFileName(parentDirectory);
        this._specialValues[SlugType.String]["[[ParentDirectoryPath]]"] =
            Path.GetDirectoryName(parentDirectory) ?? string.Empty;

        // Add special values for template info
        this._specialValues[SlugType.String]["[[TemplateName]]"] = template?.Name ?? string.Empty;
        this._specialValues[SlugType.String]["[[TemplateSafeName]]"] = template?.SafeName ?? string.Empty;
        this._specialValues[SlugType.String]["[[TemplateDescription]]"] = template?.Description ?? string.Empty;
        this._specialValues[SlugType.String]["[[TemplateVersion]]"] = template?.Version ?? string.Empty;
        this._specialValues[SlugType.String]["[[TemplateAuthor]]"] = template?.Author ?? string.Empty;

        // Computer specific special values
        this._specialValues[SlugType.String]["[[CurrentUserName]]"] = Environment.UserName;

        // Other special values
        this._specialValues[SlugType.String]["[[CurrentYear]]"] = DateTime.Now.Year.ToString();
    }

    /// <summary>
    ///     Assigns special values to a slug, if possible.
    /// </summary>
    /// <param name="slug">The slug.</param>
    /// <returns>The updated slug.</returns>
    public Slug AssignValuesToSlug(Slug slug)
    {
        if (slug.CurrentValue == null)
        {
            switch (slug.Type)
            {
                case SlugType.RandomGuid:
                    slug.DefaultValue = Guid.NewGuid().ToString();
                    break;
                case SlugType.String:
                    var strValue = slug.DefaultValue;
                    foreach (var (key, value) in this._specialValues[slug.Type])
                    {
                        strValue = strValue.Replace(key, value);
                    }

                    slug.DefaultValue = strValue;
                    break;
            }

            slug.CurrentValue = slug.DefaultValue;
        }

        return slug;
    }

    /// <summary>
    ///     Get all special keywords for a specific slug type.
    /// </summary>
    /// <param name="type">The slug type.</param>
    /// <returns>The special keywords.</returns>
    public List<string> GetSpecialKeywords(SlugType type)
    {
        return this._specialValues[type].Keys.ToList();
    }
}
