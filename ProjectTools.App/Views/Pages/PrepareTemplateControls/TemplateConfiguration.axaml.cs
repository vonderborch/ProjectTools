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

        this.PrepareViewModel.SlugConfiguration.Slugs.ItemsSource = this.PrepareViewModel.PreparationTemplate.Slugs
            .Where(x => x.RequiresAnyInput)
            .Select(x => x.DisplayName).ToList();

        this.PrepareViewModel.PrepareTemplate.ViewerSlugConfig.IsEnabled = true;
    }

    private List<string> SplitTextIntoList(string text)
    {
        return text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
