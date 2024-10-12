using System.Text.Json.Serialization;

namespace ProjectTools.Core.Templates;

/// <summary>
/// A slug used in a template when creating or extending a project.
/// </summary>
public class Slug
{
    /// <summary>
    /// The key used to identify where to replace the slug with the current value for the slug.
    /// </summary>
    public required string SlugKey;
    
    /// <summary>
    /// The display name of the slug.
    /// </summary>
    public required string DisplayName;
    
    /// <summary>
    /// The type of the slug.
    /// </summary>
    public required SlugType Type;

    /// <summary>
    /// The default value for the slug.
    /// </summary>
    public required object? DefaultValue;

    /// <summary>
    /// The allowed values for the slug.
    /// </summary>
    public List<object?> AllowedValues = [];

    /// <summary>
    /// The disallowed values for the slug.
    /// </summary>
    public List<object?> DisallowedValues = [];

    /// <summary>
    /// The current value for the slug.
    /// </summary>
    [JsonIgnore]
    public object? CurrentValue;
}
