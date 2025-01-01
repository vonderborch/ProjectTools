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

        PreprocessConfigurationControl preprocessConfigurationControl = new(this);
        TemplateConfigurationControl templateConfigurationControl = new(this);
        SlugConfigurationControl slugConfigurationControl = new(this);

        this.Context = new PrepareTemplateDataContext(this, preprocessConfigurationControl, slugConfigurationControl,
            templateConfigurationControl);

        preprocessConfigurationControl.DataContext = this.Context;
        templateConfigurationControl.DataContext = this.Context;
        slugConfigurationControl.DataContext = this.Context;

        this.ViewerPreprocessConfig.Content = preprocessConfigurationControl;
        this.ViewerSlugConfig.Content = slugConfigurationControl;
        this.ViewerTemplateConfig.Content = templateConfigurationControl;
    }
}
