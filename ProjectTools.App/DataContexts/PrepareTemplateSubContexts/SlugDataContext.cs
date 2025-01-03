#region

using System;
using System.Collections.Generic;
using System.Linq;
using ProjectTools.Core.Templates;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.PrepareTemplateSubContexts;

/// <summary>
///     The data context for the slug control.
/// </summary>
public class SlugDataContext : ReactiveObject
{
    /// <summary>
    ///     A list of properties to update when something changes with what slug we are editing.
    /// </summary>
    private readonly string[] _propertiesToUpdate =
    {
        nameof(CurrentSlugName),
        nameof(CurrentSlugKey),
        nameof(CurrentSlugDefaultValue),
        nameof(CurrentSlugAllowedValues),
        nameof(CurrentSlugDisallowedValues),
        nameof(CurrentSlugSearchStrings),
        nameof(CurrentSlugRequiresUserInput),
        nameof(CurrentSlugSelectedType),
        nameof(SlugTypeSpecialValues),
        nameof(TemplateSlugsNames),
        nameof(CurrentEditingSlugName),
        nameof(CurrentSlugEnableSlugTypePanel),
        nameof(NonGuidOptionsPanelEnabled)
    };

    /// <summary>
    ///     A map of slug names by type.
    /// </summary>
    private readonly Dictionary<SlugType, string> _slugNamesByType;

    /// <summary>
    ///     A map of slug types by name.
    /// </summary>
    private readonly Dictionary<string, SlugType> _slugTypesByName;

    /// <summary>
    ///     A map of special values by slug type.
    /// </summary>
    private readonly Dictionary<SlugType, string> _specialValuesBySlugType;

    /// <summary>
    ///     The current editing slug name.
    /// </summary>
    private string _currentEditingSlugName;

    /// <summary>
    ///     The current slug we are editing.
    /// </summary>
    private PreparationSlug? _currentSlug;

    private string? _currentSlugEditingName;

    /// <summary>
    ///     Whether the slug type panel is enabled.
    /// </summary>
    private bool _currentSlugEnableSlugTypePanel;

    /// <summary>
    ///     The new slug counter.
    /// </summary>
    private ulong _newSlugCounter;

    /// <summary>
    ///     Whether the non-guid options panel is enabled.
    /// </summary>
    private bool _nonGuidOptionsPanelEnabled;

    /// <summary>
    ///     The slug cache.
    /// </summary>
    private Dictionary<string, PreparationSlug> _slugCache;

    /// <summary>
    ///     A list of slug types.
    /// </summary>
    private List<string> _slugTypes;

    /// <summary>
    ///     The special values for the currently selected slug type.
    /// </summary>
    private string _slugTypeSpecialValues;

    /// <summary>
    ///     The slugs for the current template.
    /// </summary>
    private List<string> _templateSlugsNames;

    /// <summary>
    ///     The parent context.
    /// </summary>
    public PrepareTemplateDataContext ParentContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SlugDataContext" /> class.
    /// </summary>
    /// <param name="parentContext">The parent context.</param>
    public SlugDataContext(PrepareTemplateDataContext parentContext)
    {
        this.ParentContext = parentContext;

        this._slugTypesByName =
            Enum.GetValues(typeof(SlugType)).Cast<SlugType>().ToDictionary(x => x.ToString(), x => x);
        this._slugNamesByType =
            Enum.GetValues(typeof(SlugType)).Cast<SlugType>().ToDictionary(x => x, x => x.ToString());

        this._specialValuesBySlugType = new Dictionary<SlugType, string>();
        var specialValueHandler = new SpecialValueHandler("C://my_fake_directory//sub_directory", "fake_proj", null);
        foreach (var slugType in Enum.GetValues<SlugType>())
        {
            var messageExtra = "";
            var specialKeywords = specialValueHandler.GetSpecialKeywords(slugType);
            if (specialKeywords.Count > 0)
            {
                messageExtra = string.Join(Environment.NewLine, specialKeywords);
            }

            this._specialValuesBySlugType.Add(slugType, messageExtra);
        }

        this._slugTypes = new List<string>();
        this._slugTypes = Enum.GetNames(typeof(SlugType)).Select(x => x.ToString()).ToList();

        this._templateSlugsNames = new List<string>();
        this._slugCache = new Dictionary<string, PreparationSlug>();
    }

    /// <summary>
    ///     All of the available slug types.
    /// </summary>
    public List<string> AvailableSlugTypes
    {
        get => this._slugTypes;
        set => this.RaiseAndSetIfChanged(ref this._slugTypes, value);
    }

    /// <summary>
    ///     The current editing slug name.
    /// </summary>
    public string? CurrentEditingSlugName
    {
        get => this._currentEditingSlugName;
        set => this.RaiseAndSetIfChanged(ref this._currentEditingSlugName, value);
    }

    /// <summary>
    ///     The current slug allowed values.
    /// </summary>
    public string CurrentSlugAllowedValues
    {
        get
        {
            if (this._currentSlug is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._currentSlug.AllowedValues);
        }
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.AllowedValues =
                    value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(x => (object?)x)
                        .ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The current slug default value.
    /// </summary>
    public string CurrentSlugDefaultValue
    {
        get => this._currentSlug?.DefaultValue?.ToString() ?? string.Empty;
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.DefaultValue = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The current slug disallowed values.
    /// </summary>
    public string CurrentSlugDisallowedValues
    {
        get
        {
            if (this._currentSlug is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._currentSlug.DisallowedValues);
        }
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.DisallowedValues =
                    value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(x => (object?)x)
                        .ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     Whether the slug type panel is enabled.
    /// </summary>
    public bool CurrentSlugEnableSlugTypePanel
    {
        get => this._currentSlugEnableSlugTypePanel;
        set => this.RaiseAndSetIfChanged(ref this._currentSlugEnableSlugTypePanel, value);
    }

    /// <summary>
    ///     The current slug key.
    /// </summary>
    public string CurrentSlugKey
    {
        get => this._currentSlug?.SlugKey ?? string.Empty;
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.SlugKey = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The current slug name.
    /// </summary>
    public string CurrentSlugName
    {
        get => this._currentSlug?.DisplayName ?? string.Empty;
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.DisplayName = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     Whether the current slug requires user input.
    /// </summary>
    public bool CurrentSlugRequiresUserInput
    {
        get => this._currentSlug?.RequiresUserInput ?? false;
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.RequiresUserInput = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The current slug search strings.
    /// </summary>
    public string CurrentSlugSearchStrings
    {
        get
        {
            if (this._currentSlug is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._currentSlug.SearchStrings);
        }
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.SearchStrings =
                    value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The currently selected slug type.
    /// </summary>
    public string CurrentSlugSelectedType
    {
        get
        {
            if (this._currentSlug is null)
            {
                return string.Empty;
            }

            return this._slugNamesByType[this._currentSlug.Type];
        }
        set
        {
            if (this._currentSlug is not null)
            {
                this._currentSlug.Type = this._slugTypesByName[value];
                this.NonGuidOptionsPanelEnabled = this._currentSlug.Type != SlugType.RandomGuid;
                this.SlugTypeSpecialValues = this._specialValuesBySlugType[this._currentSlug.Type];
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     Whether the non-guid options panel is enabled.
    /// </summary>
    public bool NonGuidOptionsPanelEnabled
    {
        get => this._nonGuidOptionsPanelEnabled;
        set => this.RaiseAndSetIfChanged(ref this._nonGuidOptionsPanelEnabled, value);
    }

    /// <summary>
    ///     The slug type special values.
    /// </summary>
    public string SlugTypeSpecialValues
    {
        get => this._slugTypeSpecialValues;
        set => this.RaiseAndSetIfChanged(ref this._slugTypeSpecialValues, value);
    }

    /// <summary>
    ///     The slugs for the current template.
    /// </summary>
    public List<string> TemplateSlugsNames
    {
        get => this._templateSlugsNames;
        set => this.RaiseAndSetIfChanged(ref this._templateSlugsNames, value);
    }

    /// <summary>
    ///     Adds a new slug.
    /// </summary>
    public string AddSlug()
    {
        if (this.ParentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        var newSlug = new PreparationSlug
        {
            DisplayName = $"NewSlug{this._newSlugCounter}",
            SlugKey = $"NewSlug{this._newSlugCounter}",
            RequiresUserInput = true,
            Type = SlugType.String,
            DefaultValue = string.Empty,
            CustomSlug = true
        };
        this._newSlugCounter++;

        this.ParentContext.PreparationTemplate.Slugs.Add(newSlug);
        this._slugCache.Add(newSlug.DisplayName, newSlug);

        UpdateSlugNames();
        return newSlug.DisplayName;
    }

    /// <summary>
    ///     Clears the context.
    /// </summary>
    public void ClearContext()
    {
        this._currentSlug = null;
        this.NonGuidOptionsPanelEnabled = false;
        this.CurrentSlugEnableSlugTypePanel = false;
        this.CurrentEditingSlugName = string.Empty;
        this._currentSlugEditingName = string.Empty;
        this._slugCache = new Dictionary<string, PreparationSlug>();
        UpdateSlugNames();
        UpdateContext();
    }

    /// <summary>
    ///     Deletes the currently edited slug.
    /// </summary>
    public void DeleteCurrentSlug()
    {
        if (this.ParentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        if (this._currentSlug is not null)
        {
            this._slugCache.Remove(this._currentSlug.DisplayName);
            this.ParentContext.PreparationTemplate.Slugs.Remove(this._currentSlug);
            UpdateSlugNames();
            this._currentSlug = null;
            this.CurrentEditingSlugName = string.Empty;
            this._currentSlugEditingName = string.Empty;
            UpdateContext();
        }
    }

    /// <summary>
    ///     Enables the context.
    /// </summary>
    /// <exception cref="Exception">Raised if the preparation template is null.</exception>
    public void EnableContext()
    {
        if (this.ParentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        this._slugCache = this.ParentContext.PreparationTemplate.Slugs.ToDictionary(x => x.DisplayName, x => x);
        this.CurrentEditingSlugName = string.Empty;

        UpdateSlugNames();

        UpdateContext();
    }

    /// <summary>
    ///     Selects the slug context.
    /// </summary>
    /// <param name="slugToEdit">The slug to edit.</param>
    /// <exception cref="Exception">Raised if the preparation template is null.</exception>
    public void SelectSlugContext(string slugToEdit)
    {
        if (this.ParentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        if (!string.IsNullOrEmpty(slugToEdit) && this._currentSlug is not null)
        {
            if (this._currentSlugEditingName != this._currentSlug.DisplayName)
            {
                this._slugCache.Add(this._currentSlug.DisplayName, this._currentSlug);
                this._slugCache.Remove(this._currentSlugEditingName);

                this.TemplateSlugsNames = this._slugCache.Where(x => x.Value.RequiresAnyInput)
                    .Select(x => x.Value.DisplayName).Order()
                    .ToList();
            }
        }

        if (string.IsNullOrEmpty(slugToEdit) || !this._slugCache.TryGetValue(slugToEdit, out var slug))
        {
            this._currentEditingSlugName = string.Empty;
            UpdateContext();
            return;
        }

        this._currentSlug = slug;
        this._currentSlugEditingName = slugToEdit;
        this._slugTypeSpecialValues = this._specialValuesBySlugType[slug.Type];
        this.CurrentSlugEnableSlugTypePanel = slug.CustomSlug;
        this.NonGuidOptionsPanelEnabled = slug.Type != SlugType.RandomGuid;

        UpdateContext();
    }

    /// <summary>
    ///     Goes through and marks that each property is updated.
    /// </summary>
    private void UpdateContext()
    {
        foreach (var property in this._propertiesToUpdate)
        {
            this.RaisePropertyChanged(property);
        }
    }

    /// <summary>
    ///     Updates the slug names.
    /// </summary>
    private void UpdateSlugNames()
    {
        this.TemplateSlugsNames = this._slugCache.Where(x => x.Value.RequiresAnyInput).Select(x => x.Value.DisplayName)
            .Order()
            .ToList();
    }
}
