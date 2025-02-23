#region

using System.Diagnostics;
using ProjectTools.Core.Helpers;

#endregion

namespace ProjectTools.Core.Templates;

/// <summary>
///     A slug used when creating a new template.
/// </summary>
[DebuggerDisplay("{DisplayName} - {CurrentValue}")]
public class PreparationSlug : Slug
{
    /// <summary>
    ///     True if the slug is a custom one, False otherwise.
    /// </summary>
    public bool CustomSlug = false;

    /// <summary>
    ///     True if we need to ask about this preparation slug _at all_, false otherwise.
    /// </summary>
    public bool RequiresAnyInput = true;

    /// <summary>
    ///     The strings we search for to replace with the SlugKey.
    /// </summary>
    public List<string> SearchStrings = [];

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

    /// <summary>
    ///     Validates the slug.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Raised if validation fails for some reason.</exception>
    public override void Validate()
    {
        var isEmpty = string.IsNullOrWhiteSpace(this.DefaultValue);

        if (!this.AllowEmptyValues && isEmpty)
        {
            throw new ArgumentOutOfRangeException(nameof(this.DefaultValue),
                $"The default value for the slug '{this.DisplayName}' cannot be empty.");
        }

        if (this.AllowedValues.Count > 0 && !this.AllowedValues.IsContained(this.DefaultValue, this.CaseSensitive) &&
            !(this.AllowEmptyValues && isEmpty))
        {
            throw new ArgumentOutOfRangeException(nameof(this.DefaultValue),
                $"The default value '{this.DefaultValue}' is not allowed for the slug '{this.DisplayName}'.");
        }

        if (this.DisallowedValues.Count > 0 &&
            !this.DisallowedValues.IsContained(this.DefaultValue, this.CaseSensitive))
        {
            throw new ArgumentOutOfRangeException(nameof(this.DefaultValue),
                $"The default value '{this.DefaultValue}' is not allowed for the slug '{this.DisplayName}'.");
        }

        switch (this.Type)
        {
            case SlugType.Boolean:
                if (!bool.TryParse(this.DefaultValue, out _))
                {
                    throw new ArgumentOutOfRangeException(nameof(this.DefaultValue),
                        $"The default value '{this.DefaultValue}' is not a valid boolean value for the slug '{this.DisplayName}'.");
                }

                break;
            case SlugType.Int:
                if (!int.TryParse(this.DefaultValue, out _))
                {
                    throw new ArgumentOutOfRangeException(nameof(this.DefaultValue),
                        $"The default value '{this.DefaultValue}' is not a valid integer value for the slug '{this.DisplayName}'.");
                }

                break;
        }
    }
}
