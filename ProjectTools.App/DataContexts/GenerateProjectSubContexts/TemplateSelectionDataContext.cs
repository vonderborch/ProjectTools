#region

using System.Collections.Generic;
using System.Linq;
using ProjectTools.Core;
using ProjectTools.Core.Templates;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.GenerateProjectSubContexts;

/// <summary>
///     The data context for the template selection control.
/// </summary>
public class TemplateSelectionDataContext : ReactiveObject
{
    /// <summary>
    ///     A list of properties to update when the selected template changes.
    /// </summary>
    private readonly string[] PropertiesToUpdate = new[]
    {
        nameof(SelectedTemplateDescription),
        nameof(SelectedTemplateAuthor),
        nameof(SelectedTemplateVersion),
        nameof(SelectedTemplateSource)
    };

    /// <summary>
    ///     The available templates.
    /// </summary>
    private List<string> _availableTemplateNamesNames;

    /// <summary>
    ///     The map of the available templates.
    /// </summary>
    private Dictionary<string, LocalTemplateInfo> _availableTemplates;

    /// <summary>
    ///     The selected template.
    /// </summary>
    private LocalTemplateInfo? _selectedTemplate;

    /// <summary>
    ///     The name of the selected template.
    /// </summary>
    private string _selectedTemplateName;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateSelectionDataContext" /> class.
    /// </summary>
    public TemplateSelectionDataContext()
    {
        this._availableTemplateNamesNames = new List<string>();
        this._selectedTemplateName = string.Empty;

        var localTemplates = new LocalTemplates();
        this._availableTemplates = localTemplates.Templates.ToDictionary(x => x.Name);
        this.AvailableTemplateNames = this._availableTemplates.Keys.Order().ToList();
        this.SelectedTemplateName = this.AvailableTemplateNames.FirstOrDefault() ?? string.Empty;
    }

    /// <summary>
    ///     The author of the selected template.
    /// </summary>
    public string SelectedTemplateAuthor => this._selectedTemplate?.Template.Author ?? string.Empty;

    /// <summary>
    ///     The description of the selected template.
    /// </summary>
    public string SelectedTemplateDescription => this._selectedTemplate?.Template.Description ?? string.Empty;

    /// <summary>
    ///     The source of the selected template.
    /// </summary>
    public string SelectedTemplateSource
    {
        get
        {
            if (this._selectedTemplate is null)
            {
                return string.Empty;
            }

            return this._selectedTemplate.Value.IsLocalOnlyTemplate
                ? "Local Template"
                : this._selectedTemplate.Value.Repo;
        }
    }

    /// <summary>
    ///     The version of the selected template.
    /// </summary>
    public string SelectedTemplateVersion => this._selectedTemplate?.Template.Version ?? string.Empty;

    /// <summary>
    ///     The available templates.
    /// </summary>
    public List<string> AvailableTemplateNames
    {
        get => this._availableTemplateNamesNames;
        set => this.RaiseAndSetIfChanged(ref this._availableTemplateNamesNames, value);
    }

    /// <summary>
    ///     The name of the selected template.
    /// </summary>
    public string SelectedTemplateName
    {
        get => this._selectedTemplateName;
        set
        {
            this.RaiseAndSetIfChanged(ref this._selectedTemplateName, value);
            if (this._availableTemplates.TryGetValue(value, out var templateInfo))
            {
                this._selectedTemplate = templateInfo;
            }
            else
            {
                this._selectedTemplate = null;
            }

            UpdateProperties();
        }
    }

    public void Refresh()
    {
        var localTemplates = new LocalTemplates();
        this._availableTemplates = localTemplates.Templates.ToDictionary(x => x.Name);
        this.AvailableTemplateNames = this._availableTemplates.Keys.Order().ToList();
        this.SelectedTemplateName = this.AvailableTemplateNames.FirstOrDefault() ?? string.Empty;
        UpdateProperties();
    }

    /// <summary>
    ///     Executes to indicate that the properties have updated.
    /// </summary>
    private void UpdateProperties()
    {
        foreach (var property in this.PropertiesToUpdate)
        {
            this.RaisePropertyChanged(property);
        }
    }
}
