using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;

namespace ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

public partial class TemplateConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly PrepareTemplatePage _parentPage;

    public TemplateConfigurationControl(PrepareTemplatePage parent)
    {
        this._parentPage = parent;
        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    public PrepareTemplateDataContext Context => this._parentPage.Context;

    private void ButtonSaveTemplateSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
