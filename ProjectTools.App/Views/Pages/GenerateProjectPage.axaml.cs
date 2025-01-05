#region

using Avalonia.Controls;
using ProjectTools.App.DataContexts;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.App.Views.Pages.GenerateProjectControls;

#endregion

namespace ProjectTools.App.Views.Pages;

[PageRegistration("Generate Project", Page.GenerateProject)]
public partial class GenerateProjectPage : UserControl
{
    public GenerateProjectDataContext Context;

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
