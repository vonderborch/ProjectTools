using System.Text.Json.Serialization;

namespace ProjectTools.Core.Templates;

/// <summary>
///     A slug used in a template when creating or extending a project.
/// </summary>
public class Slug
{
    /// <summary>
    ///     The allowed values for the slug.
    /// </summary>
    public List<object?> AllowedValues = [];

    /// <summary>
    ///     The current value for the slug.
    /// </summary>
    [JsonIgnore] public object? CurrentValue = null;

    /// <summary>
    ///     The default value for the slug.
    /// </summary>
    public object? DefaultValue = null;

    /// <summary>
    ///     The disallowed values for the slug.
    /// </summary>
    public List<object?> DisallowedValues = [];

    /// <summary>
    ///     The display name of the slug.
    /// </summary>
    public required string DisplayName;

    /// <summary>
    ///     True if the slug requires user input, false otherwise.
    /// </summary>
    public bool RequiresUserInput = false;

    /// <summary>
    ///     The key used to identify where to replace the slug with the current value for the slug.
    /// </summary>
    public required string SlugKey;

    /// <summary>
    ///     The type of the slug.
    /// </summary>
    public required SlugType Type;

    /// <summary>
    ///     The actual slug key used in files.
    /// </summary>
    [JsonIgnore]
    public string ActualSlugKey => $"[[{this.SlugKey}]]";
}
