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
    /// </summary>
    /// <param name="parentDirectory"></param>
    /// <param name="template"></param>
    public SpecialValueHandler(string parentDirectory, Template? template)
    {
        foreach (var slugType in Enum.GetValues<SlugType>())
        {
            this._specialValues[slugType] = new Dictionary<string, string>();
        }


        //// String Special Values
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
    }

    public List<string> GetSpecialKeywords(SlugType type)
    {
        return this._specialValues[type].Keys.ToList();
    }

    public void AddSlugValueToSpecialValues(SlugType slugType, string slug, string value)
    {
        if (!this._specialValues.ContainsKey(slugType))
        {
            this._specialValues[slugType] = new Dictionary<string, string>();
        }

        this._specialValues[slugType].TryAdd(slug, value);
    }

    public Slug AssignValuesToSlug(Slug slug)
    {
        if (slug.CurrentValue == null)
        {
            switch (slug.Type)
            {
                case SlugType.RandomGuid:
                    slug.DefaultValue = Guid.NewGuid();
                    break;
                case SlugType.String:
                    var strValue = slug.DefaultValue?.ToString() ?? string.Empty;
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
}
