using Avalonia.Controls;
using ProjectTools.App.DataContexts;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

namespace ProjectTools.App.Views.Pages;

[PageRegistration("Prepare Template", Page.PrepareTemplate)]
public partial class PrepareTemplatePage : UserControl
{
    /// <summary>
    ///     The data context.
    /// </summary>
    public PrepareTemplateDataContext Context;

    public PrepareTemplatePage()
    {
        InitializeComponent();

        this.Context = new PrepareTemplateDataContext(this.SelectableTextBlockLog);
        PreprocessConfigurationControl preprocessConfigurationControl = new(this)
        {
            DataContext = this.Context
        };
        TemplateConfigurationControl templateConfigurationControl = new(this)
        {
            DataContext = this.Context
        };
        SlugConfigurationControl slugConfigurationControl = new(this)
        {
            DataContext = this.Context
        };

        this.ViewerPreprocessConfig.Content = preprocessConfigurationControl;
        this.ViewerSlugConfig.Content = slugConfigurationControl;
        this.ViewerTemplateConfig.Content = templateConfigurationControl;
    }
}
