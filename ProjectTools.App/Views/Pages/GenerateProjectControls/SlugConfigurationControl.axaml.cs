#region

using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;

#endregion

namespace ProjectTools.App.Views.Pages.GenerateProjectControls;

public partial class SlugConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly GenerateProjectPage _parentPage;

    public SlugConfigurationControl(GenerateProjectPage parent)
    {
        this._parentPage = parent;

        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    private GenerateProjectDataContext Context => this._parentPage.Context;

    private void ButtonGenerateProject_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
