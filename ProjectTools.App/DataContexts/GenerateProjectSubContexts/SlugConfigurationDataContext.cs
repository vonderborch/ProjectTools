#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.GenerateProjectSubContexts;

/// <summary>
///     The slug configuration data context.
/// </summary>
public class SlugConfigurationDataContext : ReactiveObject
{
    /// <summary>
    ///     The properties to update.
    /// </summary>
    private readonly string[] _propertiesToUpdate =
    {
        nameof(CurrentSlugName),
        nameof(SlugNamesToEdit),
        nameof(CurrentSlugValue),
        nameof(HasAllowedValues),
        nameof(AllowedValues),
        nameof(DisallowedValues),
        nameof(HasDisallowedValues),
        nameof(CurrentSlugType)
    };

    private Slug? _currentSlug;

    /// <summary>
    ///     The current slug name.
    /// </summary>
    private string _currentSlugName;

    /// <summary>
    ///     The slugs to edit.
    /// </summary>
    private List<string> _slugNameNamesToEdit = new();

    /// <summary>
    ///     The slugs for the template.
    /// </summary>
    private Dictionary<string, Slug> _slugs;

    /// <summary>
    ///     The special value handler.
    /// </summary>
    private SpecialValueHandler? _specialValueHandler;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SlugConfigurationDataContext" /> class.
    /// </summary>
    /// <param name="parentContext">The parent context.</param>
    public SlugConfigurationDataContext(GenerateProjectDataContext parentContext)
    {
        this.ParentContext = parentContext;

        this._currentSlugName = string.Empty;
        this._slugs = new Dictionary<string, Slug>();
        this.ProjectConfigFile = string.Empty;
    }

    /// <summary>
    ///     The allowed values.
    /// </summary>
    public string AllowedValues
    {
        get
        {
            if (this._currentSlug is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._currentSlug.AllowedValues);
        }
    }

    /// <summary>
    ///     The current slug type.
    /// </summary>
    public string CurrentSlugType => this._currentSlug?.Type.ToString() ?? string.Empty;

    /// <summary>
    ///     The disallowed values.
    /// </summary>
    public string DisallowedValues
    {
        get
        {
            if (this._currentSlug is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._currentSlug.DisallowedValues);
        }
    }

    /// <summary>
    ///     Whether the current slug has allowed values.
    /// </summary>
    public bool HasAllowedValues
    {
        get
        {
            if (this._currentSlug is null)
            {
                return false;
            }

            return this._currentSlug.AllowedValues.Count > 0;
        }
    }

    /// <summary>
    ///     Whether the current slug has disallowed values.
    /// </summary>
    public bool HasDisallowedValues
    {
        get
        {
            if (this._currentSlug is null)
            {
                return false;
            }

            return this._currentSlug.DisallowedValues.Count > 0;
        }
    }

    /// <summary>
    ///     The parent context.
    /// </summary>
    public GenerateProjectDataContext ParentContext { get; }

    /// <summary>
    ///     The current slug name.
    /// </summary>
    public string CurrentSlugName
    {
        get => this._currentSlugName;
        set
        {
            this.RaiseAndSetIfChanged(ref this._currentSlugName, value);
            if (this._slugNameNamesToEdit.Contains(value))
            {
                this._currentSlug = this._slugs[value];
            }
            else
            {
                this._currentSlug = null;
            }

            RefreshProperties();
        }
    }

    /// <summary>
    ///     The current slug value.
    /// </summary>
    public string CurrentSlugValue
    {
        get
        {
            if (this._currentSlug is null)
            {
                return string.Empty;
            }

            return this._currentSlug.CurrentValue?.ToString() ?? string.Empty;
        }
        set
        {
            if (this._currentSlug is not null)
            {
                this.RaiseAndSetIfChanged(ref this._currentSlug.CurrentValue, value);
            }
        }
    }

    /// <summary>
    ///     The project generation config file.
    /// </summary>
    public string ProjectConfigFile { get; private set; }

    /// <summary>
    ///     The slugs to edit.
    /// </summary>
    public List<string> SlugNamesToEdit
    {
        get => this._slugNameNamesToEdit;
        set => this.RaiseAndSetIfChanged(ref this._slugNameNamesToEdit, value);
    }

    /// <summary>
    ///     Enables the context.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void EnableContext()
    {
        ResetContext();

        // Validation
        if (this.ParentContext.TemplateInfo is null)
        {
            throw new Exception("Template info is null");
        }

        if (this.ParentContext.Template is null)
        {
            throw new Exception("Template is null");
        }

        // Project Generation Config File
        IOHelpers.CreateDirectoryIfNotExists(PathConstants.ProjectGenerationConfigDirectory);

        this.ProjectConfigFile =
            string.IsNullOrWhiteSpace(this.ParentContext.CoreConfigurationContext.SpecificProjectGenerationConfigFile)
                ? PathConstants.DefaultProjectGenerationConfigFileName
                : this.ParentContext.CoreConfigurationContext.SpecificProjectGenerationConfigFile;

        var projectConfigFile = Path.Combine(PathConstants.ProjectGenerationConfigDirectory, this.ProjectConfigFile);
        if (this.ParentContext.CoreConfigurationContext.RemoveExistingProjectGenerationConfigurationFile &&
            File.Exists(projectConfigFile))
        {
            File.Delete(projectConfigFile);
        }

        // Special Value Handler
        this._specialValueHandler =
            new SpecialValueHandler(this.ParentContext.CoreConfigurationContext.ParentOutputDirectory,
                this.ParentContext.CoreConfigurationContext.ProjectName, this.ParentContext.Template);

        // Slugs
        this._slugs = this.ParentContext.Template.Slugs.Select(x => x.CopySlug()).ToDictionary(x => x.DisplayName);
        this.SlugNamesToEdit =
            this._slugs.Where(x => x.Value.RequiresUserInput).Select(x => x.Value.DisplayName).ToList();

        // Update non-user interactive slugs to the default values...
        var directSlugs = GetSlugs(false);
        for (var i = 0; i < directSlugs.Count; i++)
        {
            directSlugs[i] = this._specialValueHandler.AssignValuesToSlug(directSlugs[i]);
        }

        // Set user-interactive slugs to their default values...
        var interactiveSlugs = GetSlugs(true);
        if (File.Exists(projectConfigFile))
        {
            interactiveSlugs = SlugHelpers.GetSlugsFromProjectConfigFile(projectConfigFile, interactiveSlugs);
        }

        for (var i = 0; i < interactiveSlugs.Count; i++)
        {
            interactiveSlugs[i] = this._specialValueHandler.AssignValuesToSlug(interactiveSlugs[i]);
        }

        this.CurrentSlugName = this.SlugNamesToEdit.Count > 0 ? this.SlugNamesToEdit[0] : string.Empty;
    }

    /// <summary>
    ///     Gets all of the slugs.
    /// </summary>
    /// <returns>All of the slugs.</returns>
    public List<Slug> GetAllSlugs()
    {
        return this._slugs.Values.ToList();
    }

    /// <summary>
    ///     Gets the interactive slugs.
    /// </summary>
    /// <param name="interactive">Whether the slug should require user input or not.</param>
    /// <returns>The interactive slugs.</returns>
    public List<Slug> GetSlugs(bool interactive)
    {
        return this._slugs.Where(x => x.Value.RequiresUserInput == interactive).Select(x => x.Value).ToList();
    }

    /// <summary>
    ///     Refreshes the properties.
    /// </summary>
    private void RefreshProperties()
    {
        foreach (var property in this._propertiesToUpdate)
        {
            this.RaisePropertyChanged(property);
        }
    }

    /// <summary>
    ///     Resets the context.
    /// </summary>
    public void ResetContext()
    {
        this.SlugNamesToEdit = new List<string>();
        this.CurrentSlugName = string.Empty;
        this._slugs = new Dictionary<string, Slug>();
        this._specialValueHandler = null;
        this.ProjectConfigFile = string.Empty;
        this._currentSlug = null;

        RefreshProperties();
    }

    /// <summary>
    ///     Validates the slugs.
    /// </summary>
    /// <exception cref="Exception">Raised if a slug fails validation.</exception>
    public void ValidateSlugs()
    {
        var interactiveSlugs = GetSlugs(true);
        for (var i = 0; i < interactiveSlugs.Count; i++)
        {
            if (interactiveSlugs[i].AllowedValues.Count > 0 &&
                !interactiveSlugs[i].AllowedValues.Contains(interactiveSlugs[i].CurrentValue))
            {
                throw new Exception(
                    $"The value for the slug '{interactiveSlugs[i].DisplayName}' is not in the allowed values list.");
            }

            if (interactiveSlugs[i].DisallowedValues.Count > 0 &&
                interactiveSlugs[i].DisallowedValues.Contains(interactiveSlugs[i].CurrentValue))
            {
                throw new Exception(
                    $"The value for the slug '{interactiveSlugs[i].DisplayName}' is in the disallowed values list.");
            }
        }
    }
}
