using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ProjectTools.Core.Templates;

/// <summary>
///     A preparation template used when preparing a project for templating.
/// </summary>
[DebuggerDisplay("{Name}")]
public class PreparationTemplate : AbstractTemplate
{
    /// <summary>
    ///     The cache of slug replacement values.
    /// </summary>
    [JsonIgnore] private Dictionary<string, string>? _slugReplacementCache;

    /// <summary>
    ///     Information on slugs for this template, used to replace instances of the slug with the value of the slug.
    /// </summary>
    public List<PreparationSlug> Slugs = [];

    /// <summary>
    ///     Replacement text for the slugs.
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string> ReplacementText
    {
        get
        {
            if (this._slugReplacementCache != null)
            {
                return this._slugReplacementCache;
            }

            this._slugReplacementCache = [];
            foreach (var slug in this.Slugs)
            {
                foreach (var key in slug.SearchStrings)
                {
                    this._slugReplacementCache.Add(key, slug.ActualSlugKey);
                }
            }

            return this._slugReplacementCache;
        }
    }

    /// <summary>
    ///     A method to get the template without the slugs.
    /// </summary>
    /// <returns>The template, but without any slugs listed.</returns>
    public PreparationTemplate GetTemplateWithoutSlugs()
    {
        return new PreparationTemplate
        {
            Author = this.Author,
            Description = this.Description,
            Name = this.Name,
            PathsToRemove = this.PathsToRemove,
            PrepareExcludedPaths = this.PrepareExcludedPaths,
            PythonScriptPaths = this.PythonScriptPaths,
            RenameOnlyPaths = this.RenameOnlyPaths,
            TemplateBuilder = this.TemplateBuilder,
            TemplaterVersion = this.TemplaterVersion,
            Version = this.Version
        };
    }

    /// <summary>
    ///     Converts the preparation template to a regular template.
    /// </summary>
    /// <returns>The template.</returns>
    public Template ToTemplate()
    {
        var slugs = this.Slugs.Select(x => x.ToSlug()).ToList();
        return new Template
        {
            Author = this.Author,
            Description = this.Description,
            Name = this.Name,
            PathsToRemove = this.PathsToRemove,
            PrepareExcludedPaths = this.PrepareExcludedPaths,
            PythonScriptPaths = this.PythonScriptPaths,
            RenameOnlyPaths = this.RenameOnlyPaths,
            TemplateBuilder = this.TemplateBuilder,
            Version = this.Version,
            Slugs = slugs
        };
    }
}
