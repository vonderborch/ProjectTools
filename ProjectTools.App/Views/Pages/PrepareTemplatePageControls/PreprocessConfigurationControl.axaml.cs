#region

using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ProjectTools.App.DataContexts;
using ProjectTools.App.Dialogs.YesNoDialogBox;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;

#endregion

namespace ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

/// <summary>
///     The control for the preprocess configuration.
/// </summary>
public partial class PreprocessConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly PrepareTemplatePage _parentPage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PreprocessConfigurationControl" /> class.
    /// </summary>
    /// <param name="parent">The parent page.</param>
    public PreprocessConfigurationControl(PrepareTemplatePage parent)
    {
        this._parentPage = parent;
        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    private PrepareTemplateDataContext Context => this._parentPage.Context;

    /// <summary>
    ///     Event handler for when the button to select the output directory is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private async void ButtonOutputDirectory_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Directory to Prepare as Template" });

        if (folders.Count >= 1)
        {
            var folder = folders[0];
            this.Context.PreprocessDataContext.OutputDirectory = folder.TryGetLocalPath();
        }
    }

    /// <summary>
    ///     Event handler for when the button to preprocess the template is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private async void ButtonPreProcess_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            ValidatePreProcessParameters();

            // Get the template builder we need to use
            this.Context.TemplateBuilder =
                this.Context.TemplatePreparer.GetTemplateBuilderForOption(
                    this.Context.PreprocessDataContext.SelectedTemplateBuilder,
                    this.Context.PreprocessDataContext.Directory);

            // Check if we already have a template or not
            this.Context.PreparationTemplate =
                JsonHelpers.DeserializeFromFile<PreparationTemplate>(this.Context.TemplateSettingsFile);
            if (this.Context.PreparationTemplate is not null)
            {
                if (this.Context.PreparationTemplate.TemplaterVersion < TemplateConstants.MinSupportedTemplateVersion ||
                    this.Context.PreparationTemplate.TemplaterVersion > TemplateConstants.MaxSupportedTemplateVersion)
                {
                    this.Context.WriteLog(
                        $"Template version {this.Context.PreparationTemplate.TemplaterVersion} is not supported. Current supported versions are {TemplateConstants.MinSupportedTemplateVersion} to {TemplateConstants.MaxSupportedTemplateVersion}");
                    this.Context.PreparationTemplate = null;
                }
            }

            this.Context.HadTemplate = this.Context.PreparationTemplate is not null;
            this.Context.PreparationTemplate ??= new PreparationTemplate();
            this.Context.PreparationTemplate.TemplaterVersion = TemplateConstants.CurrentTemplateVersion;

            this.Context.WriteLog("Pre-processing successful!");
            this.Context.TemplateConfigurationEnabled = true;
        }
        catch (Exception ex)
        {
            this.Context.WriteLog($"Error While Pre-Processing Template: {ex.Message}");
            await YesNoDialogBox.Open(
                this,
                "Error",
                $"Error While Pre-Processing Template: {ex.Message}",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
        }
    }

    /// <summary>
    ///     Event handler for when the button to select the template directory is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private async void ButtonTemplateDirectory_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Directory to Prepare as Template" });

        if (folders.Count >= 1)
        {
            var folder = folders[0];
            var path = folder.TryGetLocalPath();
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            if (path.EndsWith(Path.DirectorySeparatorChar) || path.EndsWith(Path.AltDirectorySeparatorChar))
            {
                path = path.Remove(path.Length - 1);
            }
            this.Context.PreprocessDataContext.Directory = path;
            this.Context.TemplateConfigurationEnabled = false;
        }
    }

    /// <summary>
    ///     Event handler for when the template builder combo box selection changes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ComboBoxTemplateBuilders_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        this.Context.TemplateConfigurationEnabled = false;
    }

    /// <summary>
    ///     Event handler for when the text in the template directory text box changes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void TextBoxTemplateDirectory_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        this.Context.PreprocessDataContext.Directory = ((TextBox)sender).Text;
        this.Context.TemplateConfigurationEnabled = false;
    }

    /// <summary>
    ///     Validates the pre process parameters.
    /// </summary>
    /// <exception cref="Exception">Raised if a parameter is invalid.</exception>
    private void ValidatePreProcessParameters()
    {
        if (!Directory.Exists(this.Context.PreprocessDataContext.Directory))
        {
            throw new Exception("Template Directory specified does not exist!");
        }
    }
}
