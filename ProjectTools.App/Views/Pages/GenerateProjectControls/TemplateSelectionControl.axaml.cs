#region

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;
using ProjectTools.Core;

#endregion

namespace ProjectTools.App.Views.Pages.GenerateProjectControls;

/// <summary>
///     The template selection control.
/// </summary>
public partial class TemplateSelectionControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly GenerateProjectPage _parentPage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateSelectionControl" /> class.
    /// </summary>
    /// <param name="parent">The parent control.</param>
    public TemplateSelectionControl(GenerateProjectPage parent)
    {
        this._parentPage = parent;
        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    private GenerateProjectDataContext Context => this._parentPage.Context;

    private void ButtonUpdateTemplates_OnClick(object? sender, RoutedEventArgs e)
    {
        var updateResultCheck = TemplateUpdater.CheckForTemplateUpdates(forceCheck: true);
        
    }

    private void ButtonUseSelectedTemplate_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
