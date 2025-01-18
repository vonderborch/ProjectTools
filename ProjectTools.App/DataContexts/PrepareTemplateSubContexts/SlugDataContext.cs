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
    [
        nameof(CurrentSlugName),
        nameof(CurrentSlugKey),
        nameof(CurrentSlugDisallowedValues),
        nameof(CurrentSlugDefaultValue),
        nameof(CurrentSlugAllowedValues),
        nameof(CurrentSlugRequiresUserInput),
        nameof(CurrentSlugEnableSlugTypePanel),
        nameof(SlugTypeSpecialValues),
        nameof(CurrentSlugSearchStrings),
        nameof(NonGuidOptionsPanelEnabled),
        nameof(IsEditingSlug),
        nameof(CurrentSlugSelectedType),
        nameof(CurrentSlugToEditName)
    ];

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
    ///     The current slug.
    /// </summary>
    private PreparationSlug? _currentSlug;

    /// <summary>
    ///     The name of the slug currently being edited.
    /// </summary>
    private string _currentSlugToEditName = string.Empty;

    /// <summary>
    ///     The new slug counter.
    /// </summary>
    private ulong _newSlugCounter;

    /// <summary>
    ///     A list of slug types.
    /// </summary>
    private List<string> _slugTypes;

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

        this._newSlugCounter = 0;

        this._slugTypes = Enum.GetNames(typeof(SlugType)).Select(x => x.ToString()).ToList();
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
    }

    /// <summary>
    ///     The current slug.
    /// </summary>
    public PreparationSlug? CurrentSlug => this._currentSlug;

    /// <summary>
    ///     Whether the slug type panel should be enabled.
    /// </summary>
    public bool CurrentSlugEnableSlugTypePanel => this._currentSlug?.CustomSlug == true;

    /// <summary>
    ///     Whether we are editing a slug.
    /// </summary>
    public bool IsEditingSlug => this._currentSlug != null;

    /// <summary>
    ///     Whether the non-guid options panel should be enabled.
    /// </summary>
    public bool NonGuidOptionsPanelEnabled => this._currentSlug?.Type != SlugType.RandomGuid;

    /// <summary>
    ///     The special values for the current slug type.
    /// </summary>
    public string SlugTypeSpecialValues
    {
        get
        {
            if (this.CurrentSlug is null)
            {
                return string.Empty;
            }

            return this._specialValuesBySlugType[this.CurrentSlug.Type];
        }
    }

    /// <summary>
    ///     The list of possible template slugs to edit or delete.
    /// </summary>
    public List<string> TemplateSlugsNames =>
        this.ParentContext.PreparationTemplate?.Slugs.Select(x => x.DisplayName).ToList() ?? new List<string>();

    /// <summary>
    ///     All of the available slug types.
    /// </summary>
    public List<string> AvailableSlugTypes
    {
        get => this._slugTypes;
        set => this.RaiseAndSetIfChanged(ref this._slugTypes, value);
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

            RefreshContext();
        }
    }

    /// <summary>
    ///     The current slug default value.
    /// </summary>
    public string CurrentSlugDefaultValue
    {
        get => this._currentSlug?.DefaultValue.ToString() ?? string.Empty;
        set
        {
            if (this._currentSlug != null)
            {
                this._currentSlug.DefaultValue = value;
                RefreshContext();
            }
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

            RefreshContext();
        }
    }

    /// <summary>
    ///     The key of the current slug.
    /// </summary>
    public string CurrentSlugKey
    {
        get => this._currentSlug?.SlugKey ?? string.Empty;
        set
        {
            if (this._currentSlug != null)
            {
                this._currentSlug.SlugKey = value;
                RefreshContext();
            }
        }
    }

    /// <summary>
    ///     The display name of the current slug.
    /// </summary>
    public string CurrentSlugName
    {
        get => this._currentSlug?.DisplayName ?? string.Empty;
        set
        {
            if (this._currentSlug != null)
            {
                if (value != this._currentSlug.DisplayName)
                {
                    this._currentSlug.DisplayName = value;
                    this.RaisePropertyChanged(nameof(this.TemplateSlugsNames));
                    this.CurrentSlugToEditName = value;
                    this.RaisePropertyChanged();
                    SelectSlug(this._currentSlug.DisplayName);
                    RefreshContext();
                }
            }
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
            if (this._currentSlug != null)
            {
                this._currentSlug.RequiresUserInput = value;
                RefreshContext();
            }
        }
    }

    /// <summary>
    ///     The search strings for the current slug.
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

            RefreshContext();
        }
    }

    /// <summary>
    ///     The current slug type.
    /// </summary>
    public string CurrentSlugSelectedType
    {
        get => this._currentSlug?.Type.ToString() ?? string.Empty;
        set
        {
            if (this._currentSlug != null && this._slugTypesByName.ContainsKey(value))
            {
                this._currentSlug.Type = this._slugTypesByName[value];
                RefreshContext();
            }
        }
    }

    /// <summary>
    ///     The name of the slug currently being edited.
    /// </summary>
    public string CurrentSlugToEditName
    {
        get => this._currentSlugToEditName;
        set
        {
            if (value is not null)
            {
                SelectSlug(value);
            }
        }
    }

    /// <summary>
    ///     Adds a new slug.
    /// </summary>
    /// <returns>The name of the new slug.</returns>
    /// <exception cref="Exception">Raised if bad things happen.</exception>
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
        this.ParentContext.PreparationTemplate?.Slugs.Add(newSlug);

        SelectSlug(newSlug.DisplayName);

        RefreshContext(refreshSlugNames: true);
        return newSlug.DisplayName;
    }

    /// <summary>
    ///     Deletes the current slug.
    /// </summary>
    /// <exception cref="Exception">Raised if bad things happen.</exception>
    public void DeleteSlug()
    {
        if (this.ParentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        if (this._currentSlug is not null)
        {
            this.ParentContext.PreparationTemplate?.Slugs.Remove(this._currentSlug);
            this.CurrentSlugToEditName = string.Empty;
            RefreshContext();
            this.RaisePropertyChanged(nameof(this.TemplateSlugsNames));
        }
    }

    /// <summary>
    ///     Refreshes the context.
    /// </summary>
    public void RefreshContext(bool refreshSlugNames = false)
    {
        foreach (var property in this._propertiesToUpdate)
        {
            this.RaisePropertyChanged(property);
        }

        if (refreshSlugNames)
        {
            this.RaisePropertyChanged(nameof(this.TemplateSlugsNames));
        }
    }

    /// <summary>
    ///     Selects a slug.
    /// </summary>
    /// <param name="newSlug">The slug to select.</param>
    /// <returns>The name of the selected slug.</returns>
    /// <exception cref="Exception">Raised if bad things happen.</exception>
    public string SelectSlug(string newSlug)
    {
        if (this.ParentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        var slug = this.ParentContext.PreparationTemplate?.Slugs.Where(x => x.DisplayName == newSlug).FirstOrDefault();

        if (slug is not null)
        {
            this._currentSlugToEditName = newSlug;
            this._currentSlug = slug;
        }
        else
        {
            this._currentSlugToEditName = newSlug;
            this._currentSlug = null;
        }

        this.RaisePropertyChanged(nameof(this.CurrentSlugToEditName));
        return newSlug;
    }
}
