using Avalonia.Controls;
using ProjectTools.App.DataContexts;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

namespace ProjectTools.App.Views.Pages;

/// <summary>
/// The prepare template page.
/// </summary>
[PageRegistration("Prepare Template", Page.PrepareTemplate)]
public partial class PrepareTemplatePage : UserControl
{
    /// <summary>
    ///     The data context.
    /// </summary>
    public PrepareTemplateDataContext Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrepareTemplatePage"/> class.
    /// </summary>
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
