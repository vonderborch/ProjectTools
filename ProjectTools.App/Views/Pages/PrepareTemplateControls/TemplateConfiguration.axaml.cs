using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;

namespace ProjectTools.App.Views.Pages.PrepareTemplateControls;

public partial class TemplateConfiguration : UserControl
{
    public PrepareTemplateViewModel PrepareViewModel;

    public TemplateConfiguration()
    {
        InitializeComponent();
    }

    public void ResetTemplateConfigurationSettings()
    {
        this.TextTemplateName.Text = string.Empty;
        this.TextTemplateDescription.Text = string.Empty;
        this.TextTemplateVersion.Text = string.Empty;
        this.TextTemplateAuthor.Text = string.Empty;
        this.TextTemplateRenameOnlyPaths.Text = string.Empty;
        this.TextTemplatePathsToRemove.Text = string.Empty;
        this.TextTemplatePrepareExcludedPaths.Text = string.Empty;
        this.TextTemplatePythonScriptPaths.Text = string.Empty;
        this.TextTemplateInstructions.Text = string.Empty;

        this.PrepareViewModel.PrepareTemplate.ViewerTemplateConfig.IsEnabled = false;
        this.PrepareViewModel.SlugConfiguration.ResetSlugConfigurationPanel();
    }

    public void UpdateTemplateConfigurationSettings()
    {
        this.TextTemplateName.Text =
            this.PrepareViewModel.PreparationTemplate.Name;
        this.TextTemplateDescription.Text =
            this.PrepareViewModel.PreparationTemplate.Description;
        this.TextTemplateVersion.Text =
            this.PrepareViewModel.PreparationTemplate.Version;
        this.TextTemplateAuthor.Text =
            this.PrepareViewModel.PreparationTemplate.Author;
        this.TextTemplateRenameOnlyPaths.Text = string.Join(
            Environment.NewLine,
            this.PrepareViewModel.PreparationTemplate.RenameOnlyPaths);
        this.TextTemplatePathsToRemove.Text = string.Join(
            Environment.NewLine,
            this.PrepareViewModel.PreparationTemplate.PathsToRemove);
        this.TextTemplatePrepareExcludedPaths.Text = string.Join(
            Environment.NewLine,
            this.PrepareViewModel.PreparationTemplate.PrepareExcludedPaths);
        this.TextTemplatePythonScriptPaths.Text = string.Join(
            Environment.NewLine,
            this.PrepareViewModel.PreparationTemplate.PythonScriptPaths);
        this.TextTemplateInstructions.Text = string.Join(Environment.NewLine,
            this.PrepareViewModel.PreparationTemplate.Instructions);

        this.PrepareViewModel.PrepareTemplate.ViewerTemplateConfig.IsEnabled = true;
        this.PrepareViewModel.SlugConfiguration.ResetSlugConfigurationPanel();
    }

    private void ButtonSaveTemplateSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        if (this.PrepareViewModel is null)
        {
            throw new Exception("PrepareViewModel is null!");
        }

        this.PrepareViewModel.PreparationTemplate.Name = this.TextTemplateName.Text ?? string.Empty;
        this.PrepareViewModel.PreparationTemplate.Description = this.TextTemplateDescription.Text ?? string.Empty;
        this.PrepareViewModel.PreparationTemplate.Version = this.TextTemplateVersion.Text ?? string.Empty;
        this.PrepareViewModel.PreparationTemplate.Author = this.TextTemplateAuthor.Text ?? string.Empty;

        this.PrepareViewModel.PreparationTemplate.RenameOnlyPaths =
            SplitTextIntoList(this.TextTemplateRenameOnlyPaths.Text ?? string.Empty);
        this.PrepareViewModel.PreparationTemplate.PathsToRemove =
            SplitTextIntoList(this.TextTemplatePathsToRemove.Text ?? string.Empty);
        this.PrepareViewModel.PreparationTemplate.PrepareExcludedPaths =
            SplitTextIntoList(this.TextTemplatePrepareExcludedPaths.Text ?? string.Empty);
        this.PrepareViewModel.PreparationTemplate.PythonScriptPaths =
            SplitTextIntoList(this.TextTemplatePythonScriptPaths.Text ?? string.Empty);
        this.PrepareViewModel.PreparationTemplate.Instructions =
            SplitTextIntoList(this.TextTemplateInstructions.Text ?? string.Empty);

        this.PrepareViewModel.SlugConfiguration.UpdateSlugConfigurationPanel();
    }

    private List<string> SplitTextIntoList(string text)
    {
        return text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
