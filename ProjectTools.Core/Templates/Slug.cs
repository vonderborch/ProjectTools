#region

using System.Diagnostics;
using System.Text.Json.Serialization;
using ProjectTools.Core.Helpers;

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
    public List<string> AllowedValues = [];

    /// <summary>
    ///     Whether to allow empty values or not.
    /// </summary>
    public bool AllowEmptyValues = false;

    /// <summary>
    ///     Whether comparisons to allowed values and disallowed values are case sensitive or not.
    /// </summary>
    public bool CaseSensitive = true;

    /// <summary>
    ///     The current value for the slug.
    /// </summary>
    [JsonIgnore] public string CurrentValue;

    /// <summary>
    ///     The default value for the slug.
    /// </summary>
    public string DefaultValue;
    
    /// <summary>
    ///     A description of the slug.
    /// </summary>
    public string Description = string.Empty;

    /// <summary>
    ///     The disallowed values for the slug.
    /// </summary>
    public List<string> DisallowedValues = [];

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

    /// <summary>
    ///     Validates the slug.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Raised if validation fails for some reason.</exception>
    public virtual void Validate()
    {
        var isEmpty = string.IsNullOrWhiteSpace(this.CurrentValue);

        if (!this.AllowEmptyValues && isEmpty)
        {
            throw new ArgumentOutOfRangeException(nameof(this.CurrentValue),
                $"The value for the slug '{this.DisplayName}' cannot be empty.");
        }

        if (this.AllowedValues.Count > 0 && !this.AllowedValues.IsContained(this.CurrentValue, this.CaseSensitive) &&
            !(this.AllowEmptyValues && isEmpty))
        {
            throw new ArgumentOutOfRangeException(nameof(this.CurrentValue),
                $"The value '{this.CurrentValue}' is not allowed for the slug '{this.DisplayName}'.");
        }

        if (this.DisallowedValues.Count > 0 &&
            !this.DisallowedValues.IsContained(this.CurrentValue, this.CaseSensitive))
        {
            throw new ArgumentOutOfRangeException(nameof(this.CurrentValue),
                $"The value '{this.CurrentValue}' is not allowed for the slug '{this.DisplayName}'.");
        }

        switch (this.Type)
        {
            case SlugType.Boolean:
                if (!bool.TryParse(this.CurrentValue, out _))
                {
                    throw new ArgumentOutOfRangeException(nameof(this.CurrentValue),
                        $"The value '{this.CurrentValue}' is not a valid boolean value for the slug '{this.DisplayName}'.");
                }

                break;
            case SlugType.Int:
                if (!int.TryParse(this.CurrentValue, out _))
                {
                    throw new ArgumentOutOfRangeException(nameof(this.CurrentValue),
                        $"The value '{this.CurrentValue}' is not a valid integer value for the slug '{this.DisplayName}'.");
                }

                break;
        }
    }
}
