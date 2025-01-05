#region

using Avalonia.Controls;
using ProjectTools.App.DataContexts;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.App.Views.Pages.GenerateProjectControls;

#endregion

namespace ProjectTools.App.Views.Pages;

/// <summary>
/// The generate project page.
/// </summary>
[PageRegistration("Generate Project", Page.GenerateProject)]
public partial class GenerateProjectPage : UserControl
{
    /// <summary>
    /// The data context.
    /// </summary>
    public GenerateProjectDataContext Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateProjectPage"/> class.
    /// </summary>
    public GenerateProjectPage()
    {
        InitializeComponent();

        this.Context = new GenerateProjectDataContext(this.SelectableTextBlockCoreLog,
            this.SelectableTextBlockScriptLog, this.SelectableTextBlockInstructionLog);
        TemplateSelectionControl templateSelection = new(this)
        {
            DataContext = this.Context
        };
        CoreConfigurationControl coreConfiguration = new(this)
        {
            DataContext = this.Context
        };
        SlugConfigurationControl slugConfiguration = new(this)
        {
            DataContext = this.Context
        };

        this.ViewerTemplateSelection.Content = templateSelection;
        this.ViewerCoreConfig.Content = coreConfiguration;
        this.ViewerSlugConfig.Content = slugConfiguration;
    }
}
