#region

using Avalonia.Controls;
using ProjectTools.App.DataContexts;

#endregion

namespace ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

/// <summary>
///     The control for the template configuration.
/// </summary>
public partial class TemplateConfigurationControl : UserControl
{
    /// <summary>
    ///     The parent.
    /// </summary>
    private readonly PrepareTemplatePage _parentPage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateConfigurationControl" /> class.
    /// </summary>
    /// <param name="parent">The parent page.</param>
    public TemplateConfigurationControl(PrepareTemplatePage parent)
    {
        this._parentPage = parent;
        InitializeComponent();
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    public PrepareTemplateDataContext Context => this._parentPage.Context;
}
