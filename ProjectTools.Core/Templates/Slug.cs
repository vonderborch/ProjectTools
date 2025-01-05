#region

using System.Diagnostics;
using System.Text.Json.Serialization;

#endregion

namespace ProjectTools.Core.Templates;

/// <summary>
///     A slug used in a template when creating or extending a project.
/// </summary>
[DebuggerDisplay("{DisplayName} - {CurrentValue}")]
public class Slug
{
    /// <summary>
    ///     The allowed values for the slug.
    /// </summary>
    public List<object?> AllowedValues = [];

    /// <summary>
    ///     The current value for the slug.
    /// </summary>
    [JsonIgnore] public object? CurrentValue;

    /// <summary>
    ///     The default value for the slug.
    /// </summary>
    public object? DefaultValue;

    /// <summary>
    ///     The disallowed values for the slug.
    /// </summary>
    public List<object?> DisallowedValues = [];

    /// <summary>
    ///     The display name of the slug.
    /// </summary>
    public string DisplayName = string.Empty;

    /// <summary>
    ///     True if the slug requires user input, false otherwise.
    /// </summary>
    public bool RequiresUserInput;

    /// <summary>
    ///     The key used to identify where to replace the slug with the current value for the slug.
    /// </summary>
    public string SlugKey = string.Empty;

    /// <summary>
    ///     The type of the slug.
    /// </summary>
    public SlugType Type = SlugType.String;

    /// <summary>
    ///     The actual slug key used in files.
    /// </summary>
    [JsonIgnore]
    public string ActualSlugKey => $"[[{this.SlugKey}]]";

    /// <summary>
    ///     Copies the slug.
    /// </summary>
    /// <returns>The copied slug.</returns>
    public Slug CopySlug()
    {
        var newSlug = new Slug
        {
            AllowedValues = this.AllowedValues,
            CurrentValue = this.CurrentValue,
            DefaultValue = this.DefaultValue,
            DisallowedValues = this.DisallowedValues,
            DisplayName = this.DisplayName,
            RequiresUserInput = this.RequiresUserInput,
            SlugKey = this.SlugKey,
            Type = this.Type
        };
        return newSlug;
    }
}
