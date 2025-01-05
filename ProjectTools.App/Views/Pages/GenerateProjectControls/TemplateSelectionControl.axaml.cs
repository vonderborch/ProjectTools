#region

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;
using ProjectTools.Core;
using ProjectTools.Core.TemplateRepositories;

#endregion

namespace ProjectTools.App.Views.Pages.GenerateProjectControls;

/// <summary>
///     The template selection control.
/// </summary>
public partial class TemplateSelectionControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly GenerateProjectPage _parentPage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateSelectionControl" /> class.
    /// </summary>
    /// <param name="parent">The parent control.</param>
    public TemplateSelectionControl(GenerateProjectPage parent)
    {
        this._parentPage = parent;
        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    private GenerateProjectDataContext Context => this._parentPage.Context;

    /// <summary>
    ///     The button to update templates.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private async void ButtonUpdateTemplates_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Context.LogToCore("Checking for new or updated templates...");
        var updateResultCheck = await TemplateUpdater.AsyncCheckForTemplateUpdates(forceCheck: true);
        if (updateResultCheck.TotalTemplatesNeedingDownload == 0)
        {
            this.Context.LogToCore("No templates to update or download!");
            return;
        }

        var totalTemplatesToDownload = new List<GitTemplateMetadata>();
        this.Context.LogToCore(
            $"Found {updateResultCheck.NewTemplates.Count} new templates to download ({updateResultCheck.NewTemplateSizeInMegabytes:0.000} MB)...");
        foreach (var template in updateResultCheck.NewTemplates)
        {
            totalTemplatesToDownload.Add(template);
        }

        this.Context.LogToCore(
            $"Found {updateResultCheck.UpdateableTemplates.Count} templates to update ({updateResultCheck.UpdateableTemplateSizeInMegabytes:0.000} MB)...");
        foreach (var template in updateResultCheck.UpdateableTemplates)
        {
            totalTemplatesToDownload.Add(template);
        }

        TemplateUpdater.DownloadTemplates(totalTemplatesToDownload, true);
        this.Context.LogToCore("Downloaded new/updated templates!");
        this.Context.TemplateSelectionContext.Refresh();
        this.Context.TemplateSelectionContext.Refresh();
        this.Context.TemplateInfo = null;
    }

    private void ButtonUseSelectedTemplate_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Context.TemplateInfo = null;
        var localTemplates = new LocalTemplates();
        var availableTemplates = localTemplates.Templates;

        if (availableTemplates.Count == 0)
        {
            throw new Exception("No templates available to use!");
        }

        var templateNames = localTemplates.TemplateNames;
        if (!templateNames.Contains(this.Context.TemplateSelectionContext.SelectedTemplateName))
        {
            throw new Exception(
                $"Template '{this.Context.TemplateSelectionContext.SelectedTemplateName}' not found, please run list-templates to see available templates.");
        }

        var templateToUse =
            availableTemplates.First(t => t.Name == this.Context.TemplateSelectionContext.SelectedTemplateName);
        this.Context.LogToCore(
            $"Using Template {templateToUse.Name} (Version {templateToUse.Template.Version}) to Generate Project");
        this.Context.TemplateInfo = templateToUse;

        this.Context.CoreConfigurationEnabled = true;
    }

    /// <summary>
    /// The event handler for when the list box selection changes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ListBoxAvailableTemplates_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        this.Context.TemplateInfo = null;
        this.Context.CoreConfigurationEnabled = false;
    }
}
