using System;
using System.Collections.Generic;
using System.Linq;
using ProjectTools.Core.Templates;
using ReactiveUI;

namespace ProjectTools.App.DataContexts;

/// <summary>
///     The data context for the slug configuration.
/// </summary>
public class PrepareTemplateSlugDataContext : ReactiveObject
{
    /// <summary>
    ///     A map of slug types to their special values.
    /// </summary>
    private readonly Dictionary<SlugType, string> _messageExtras;

    /// <summary>
    ///     A map of slug names to their type.
    /// </summary>
    private readonly Dictionary<string, SlugType> _slugTypesByName;

    /// <summary>
    ///     A map of slug types to their name.
    /// </summary>
    private readonly Dictionary<SlugType, string> _slugTypesByType;

    /// <summary>
    ///     The slug we are currently editing.
    /// </summary>
    private PreparationSlug? _currentEditingSlug;

    /// <summary>
    ///     The key in the _slugCache for the slug we are currently editing.
    /// </summary>
    private string _currentEditingSlugKey = string.Empty;

    /// <summary>
    ///     The cache of slugs by display name for the current template.
    /// </summary>
    private Dictionary<string, PreparationSlug> _slugCache;

    /// <summary>
    ///     A list of the names of all slug types.
    /// </summary>
    private List<string> _slugTypes;

    /// <summary>
    ///     The names of the template's slugs (that require input).
    /// </summary>
    private List<string> _templateSlugNames;

    /// <summary>
    ///     The slugs for the template.
    /// </summary>
    private Dictionary<string, PreparationSlug> _templateSlugs;

    /// <summary>
    ///     The parent context.
    /// </summary>
    public PrepareTemplateDataContext ParentContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PrepareTemplateSlugDataContext" /> class.
    /// </summary>
    /// <param name="parentContext">The parent context.</param>
    public PrepareTemplateSlugDataContext(PrepareTemplateDataContext parentContext)
    {
        this.ParentContext = parentContext;

        this._templateSlugNames = new List<string>();
        this._templateSlugs = new Dictionary<string, PreparationSlug>();


        this._slugTypes = new List<string>();
        var slugTypes = Enum.GetValues<SlugType>().ToList();
        this.SlugTypes = slugTypes.Select(x => x.ToString()).ToList();

        // Get the special values possible for each slug type
        this._messageExtras = new Dictionary<SlugType, string>();
        var specialValueHandler = new SpecialValueHandler("C://my_fake_directory//sub_directory", "fake_proj", null);
        foreach (var slugType in slugTypes)
        {
            var messageExtra = "";
            var specialKeywords = specialValueHandler.GetSpecialKeywords(slugType);
            if (specialKeywords.Count > 0)
            {
                messageExtra = string.Join(Environment.NewLine, specialKeywords);
            }

            this._messageExtras.Add(slugType, messageExtra);
        }

        // Get the slug types by name and type
        this._slugTypesByName =
            Enum.GetValues(typeof(SlugType)).Cast<SlugType>().ToDictionary(x => x.ToString(), x => x);
        this._slugTypesByType =
            Enum.GetValues(typeof(SlugType)).Cast<SlugType>().ToDictionary(x => x, x => x.ToString());
    }

    public string CurrentlyEditingSlug
    {
        get => this._currentEditingSlugKey;
        set
        {
            this.RaiseAndSetIfChanged(ref this._currentEditingSlugKey, value);
            if (this._slugCache.ContainsKey(value))
            {
                this._currentEditingSlug = this._slugCache[value];
                UpdateContext();
            }
        }
    }

    public string CurrentEditingSlugDisplayName
    {
        get => this._currentEditingSlug?.DisplayName ?? string.Empty;
        set
        {
            if (this._currentEditingSlug is not null)
            {
                this.RaiseAndSetIfChanged(ref this._currentEditingSlug.DisplayName, value);
            }
        }
    }

    /// <summary>
    ///     The available slug types.
    /// </summary>
    public List<string> SlugTypes
    {
        get => this._slugTypes;
        set => this.RaiseAndSetIfChanged(ref this._slugTypes, value);
    }

    /// <summary>
    ///     The names of the slugs for the template (that require any input).
    /// </summary>
    public List<string> TemplateSlugNames
    {
        get => this._templateSlugNames;
        set => this.RaiseAndSetIfChanged(ref this._templateSlugNames, value);
    }

    /// <summary>
    ///     Sets the context.
    /// </summary>
    /// <exception cref="Exception">Raised if the preperation template is null.</exception>
    public void SetContext()
    {
        if (this.ParentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        this._templateSlugs = this.ParentContext.PreparationTemplate.Slugs.ToDictionary(x => x.DisplayName, x => x);
        this.TemplateSlugNames = this._templateSlugs.Where(x => x.Value.RequiresAnyInput)
            .Select(x => x.Value.DisplayName).Order().ToList();
        this._currentEditingSlug = null;
        this._currentEditingSlugKey = string.Empty;
        this._slugCache = this.ParentContext.PreparationTemplate.Slugs.ToDictionary(x => x.DisplayName);

        this.RaisePropertyChanged("CurrentEditingSlugDisplayName");
    }

    /// <summary>
    ///     Clears the context.
    /// </summary>
    public void ClearContext()
    {
        this.TemplateSlugNames = new List<string>();
        this._templateSlugs = new Dictionary<string, PreparationSlug>();
        this._currentEditingSlug = null;
        this._currentEditingSlugKey = string.Empty;

        this.CurrentEditingSlugDisplayName = string.Empty;
        this.RaisePropertyChanged("CurrentEditingSlugDisplayName");
    }

    public void UpdateContext()
    {
        this.RaisePropertyChanged("CurrentEditingSlugDisplayName");
    }
}
