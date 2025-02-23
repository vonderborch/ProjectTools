#region

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;
using ProjectTools.App.Dialogs.YesNoDialogBox;
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

    /// <summary>
    ///     Event handler for when the button to add the slug is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ButtonAddSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        var slug = this.Context.SlugDataContext.AddSlug();
        this.ComboBoxSlugs.SelectedItem = slug;
    }

    /// <summary>
    ///     Event handler for when the button to delete the slug is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ButtonDeleteSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Context.SlugDataContext.DeleteSlug();
    }

    /// <summary>
    ///     Event handler for when the button to generate the template is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    /// <exception cref="InvalidOperationException">Raised in a bad scenario.</exception>
    private async void ButtonGenerateTemplate_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            this.Context.ClearLog();
            if (this.Context.PreparationTemplate is null)
            {
                throw new InvalidOperationException("Preparation template is null.");
            }

            var slugsToValidate = this.Context.PreparationTemplate.Slugs;
            foreach (var slug in slugsToValidate)
            {
                slug.Validate();
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
                var output = this.Context.TemplatePreparer.GenerateTemplate(
                    this.Context.PreprocessDataContext.Directory,
                    this.Context.PreprocessDataContext.OutputDirectory, this.Context.PreprocessDataContext.SkipCleaning,
                    this.Context.PreprocessDataContext.ForceOverwrite,
                    this.Context.PreparationTemplate, coreLogger);
                if (!string.IsNullOrEmpty(output))
                {
                    this.Context.WriteLog(output);
                }
            }
        }
        catch (Exception ex)
        {
            this.Context.WriteLog($"Error While Generating Project: {ex.Message}");
            await YesNoDialogBox.Open(
                this,
                "Error",
                $"Error While Generating Project: {ex.Message}",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
        }
    }

    /// <summary>
    ///     Event handler for when the button to generate the project is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ComboBoxSlugs_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            var slug = this.Context.SlugDataContext.SelectSlug(e.AddedItems[0].ToString());
            this.ComboBoxSlugs.SelectedItem = slug;
            this.Context.SlugDataContext.RefreshContext();
        }
    }
}
