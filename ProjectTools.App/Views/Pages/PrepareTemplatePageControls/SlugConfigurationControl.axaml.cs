#region

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;
using ProjectTools.Core.Helpers;

#endregion

namespace ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

/// <summary>
///     The control for the slug configuration.
/// </summary>
public partial class SlugConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly PrepareTemplatePage _parentPage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SlugConfigurationControl" /> class.
    /// </summary>
    /// <param name="parent">The parent page.</param>
    public SlugConfigurationControl(PrepareTemplatePage parent)
    {
        this._parentPage = parent;
        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    public PrepareTemplateDataContext Context => this._parentPage.Context;

    private void ButtonAddSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        var newSlugName = this.Context.SlugDataContext.AddSlug();
        this.ComboBoxSlugs.SelectedItem = newSlugName;
    }

    private void ButtonDeleteSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Context.SlugDataContext.DeleteCurrentSlug();
    }

    private void ButtonGenerateTemplate_OnClick(object? sender, RoutedEventArgs e)
    {
        if (this.Context.PreparationTemplate is null)
        {
            throw new InvalidOperationException("Preparation template is null.");
        }

        IOHelpers.DeleteFileIfExists(this.Context.TemplateSettingsFile);
        JsonHelpers.SerializeToFile(this.Context.TemplateSettingsFile, this.Context.PreparationTemplate);

        if (this.Context.PreprocessDataContext.WhatIf)
        {
            this.Context.WriteLog("What-If mode enabled. Template settings updated, but no template generated.");
        }
        else
        {
            Logger coreLogger = new(this.Context.WriteLog);
            var output = this.Context.TemplatePreparer.GenerateTemplate(this.Context.PreprocessDataContext.Directory,
                this.Context.PreprocessDataContext.OutputDirectory, this.Context.PreprocessDataContext.SkipCleaning,
                this.Context.PreprocessDataContext.ForceOverwrite,
                this.Context.PreparationTemplate, coreLogger);
            if (!string.IsNullOrEmpty(output))
            {
                this.Context.WriteLog(output);
            }
        }
    }

    private void ComboBoxSlugs_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            this.Context.SlugDataContext.SelectSlugContext(e.AddedItems[0].ToString());
        }
        else
        {
            this.Context.SlugDataContext.ClearContext();
        }
    }
}
