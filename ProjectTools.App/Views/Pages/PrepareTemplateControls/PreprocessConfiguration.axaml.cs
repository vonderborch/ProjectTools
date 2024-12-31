using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ProjectTools.App.Dialogs.YesNoDialogBox;
using ProjectTools.App.ViewModels;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;

namespace ProjectTools.App.Views.Pages.PrepareTemplateControls;

public partial class PreprocessConfiguration : UserControl
{
    public PrepareTemplateViewModel PrepareViewModel;

    public PreprocessConfiguration()
    {
        InitializeComponent();
    }

    private async void ButtonTemplateDirectory_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Directory to Prepare as Template" });

        if (folders.Count >= 1)
        {
            var folder = folders[0];
            this.TextTemplateDirectory.Text = folder.TryGetLocalPath();
            this.TextOutputDirectory.Text = Path.GetDirectoryName(this.TextTemplateDirectory.Text);
        }
    }

    private async void ButtonOutputDirectory_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Directory to Ouput Template To" });

        if (folders.Count >= 1)
        {
            var folder = folders[0];
            this.TextOutputDirectory.Text = folder.TryGetLocalPath();
        }
    }

    private async void ButtonPreProcess_OnClick(object? sender, RoutedEventArgs e)
    {
        this.TextPreProcessLog.Text = "";

        // Grab pre-processor parameters
        this.PrepareViewModel.TemplateDirectory = this.TextTemplateDirectory.Text ?? string.Empty;
        this.PrepareViewModel.OutputDirectory = this.TextOutputDirectory.Text ?? string.Empty;
        this.PrepareViewModel.TemplateBuilderName = this.TemplateBuilders.SelectedItem?.ToString() ?? string.Empty;
        this.PrepareViewModel.SkipCleaning = this.CheckBoxSkipCleaning.IsChecked ?? false;
        this.PrepareViewModel.ForceOverride = this.CheckBoxForce.IsChecked ?? false;
        this.PrepareViewModel.WhatIf = this.CheckBoxWhatIf.IsChecked ?? false;

        // Walk through pre-process logic
        try
        {
            ValidatePreProcessParameters();

            Preparer templater = new();
            this.PrepareViewModel.TemplateBuilder =
                templater.GetTemplateBuilderForOption(this.PrepareViewModel.TemplateBuilderName,
                    this.PrepareViewModel.TemplateDirectory);

            var templateSettingsFile = Path.Combine(this.PrepareViewModel.TemplateDirectory,
                TemplateConstants.TemplateSettingsFileName);

            this.PrepareViewModel.PreparationTemplate =
                JsonHelpers.DeserializeFromFile<PreparationTemplate>(templateSettingsFile);
            if (this.PrepareViewModel.PreparationTemplate != null)
            {
                if (this.PrepareViewModel.PreparationTemplate.TemplaterVersion <
                    TemplateConstants.MinSupportedTemplateVersion ||
                    this.PrepareViewModel.PreparationTemplate.TemplaterVersion >
                    TemplateConstants.MaxSupportedTemplateVersion)
                {
                    this.TextPreProcessLog.Text =
                        $"Template version {this.PrepareViewModel.PreparationTemplate.TemplaterVersion} is not supported. Current supported versions are {TemplateConstants.MinSupportedTemplateVersion} to {TemplateConstants.MaxSupportedTemplateVersion}";
                    this.PrepareViewModel.PreparationTemplate = null;
                }
            }

            // cleanup some final bits of logic...
            this.PrepareViewModel.HadPreparationTemplate = this.PrepareViewModel.PreparationTemplate != null;
            this.PrepareViewModel.PreparationTemplate ??= new PreparationTemplate();
            this.PrepareViewModel.PreparationTemplate.TemplaterVersion = TemplateConstants.CurrentTemplateVersion;

            // Setup the next section...
            this.PrepareViewModel.TemplateConfiguration.UpdateTemplateConfigurationSettings();

            // Enable the next section!
            this.TextPreProcessLog.Text = "Pre-Processing Successful!";
        }
        catch (Exception ex)
        {
            this.TextPreProcessLog.Text = $"Error While Pre-Processing Template: {ex.Message}";
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

    private void ValidatePreProcessParameters()
    {
        if (!Directory.Exists(this.PrepareViewModel.TemplateDirectory))
        {
            throw new Exception("Template Directory specified does not exist!");
        }
    }

    private void TextTemplateDirectory_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        this.PrepareViewModel.TemplateConfiguration.ResetTemplateConfigurationSettings();
    }

    private void TemplateBuilders_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        this.PrepareViewModel.TemplateConfiguration.ResetTemplateConfigurationSettings();
    }
}
