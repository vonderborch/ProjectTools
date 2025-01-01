using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ProjectTools.App.DataContexts;

namespace ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

public partial class PreprocessConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly PrepareTemplatePage _parentPage;

    public PreprocessConfigurationControl(PrepareTemplatePage parent)
    {
        this._parentPage = parent;
        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    public PrepareTemplateDataContext Context => this._parentPage.Context;

    private void TextBoxTemplateDirectory_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonTemplateDirectory_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Directory to Prepare as Template" });

        if (folders.Count >= 1)
        {
            var folder = folders[0];
            this.Context.TemplateDirectory = folder.TryGetLocalPath();
            this.TextBoxTemplateDirectory.Text = folder.TryGetLocalPath();
            this.TextBoxOutputDirectory.Text = Path.GetDirectoryName(this.TextBoxTemplateDirectory.Text);
        }
    }

    private void ButtonOutputDirectory_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ComboBoxTemplateBuilders_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonPreProcess_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
