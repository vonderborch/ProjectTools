#region

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ProjectTools.App.DataContexts;

#endregion

namespace ProjectTools.App.Views.Pages.GenerateProjectControls;

/// <summary>
///     The core configuration control.
/// </summary>
public partial class CoreConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly GenerateProjectPage _parentPage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CoreConfigurationControl" /> class.
    /// </summary>
    /// <param name="parent">The parent.</param>
    public CoreConfigurationControl(GenerateProjectPage parent)
    {
        this._parentPage = parent;

        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    private GenerateProjectDataContext Context => this._parentPage.Context;

    /// <summary>
    ///     Event handler for when the button to select the output directory is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private async void ButtonBrowse_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select the Parent Directory to Output to" });

        if (folders.Count >= 1)
        {
            var folder = folders[0];
            this.Context.CoreConfigurationContext.ParentOutputDirectory = folder.TryGetLocalPath();
            this.Context.SlugConfigurationEnabled = false;
        }
    }

    /// <summary>
    ///     Event handler for when the button to set the core configuration is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ButtonSetCoreConfig_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Context.SlugConfigurationEnabled = true;
    }

    /// <summary>
    ///     Event handler for when the text in the parent output directory textbox changes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void TextBoxParentOutputDirectory_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        this.Context.SlugConfigurationEnabled = false;
    }

    /// <summary>
    ///     Event handler for when the text in the project name textbox changes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void TextBoxProjectName_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        this.Context.SlugConfigurationEnabled = false;
    }
}
