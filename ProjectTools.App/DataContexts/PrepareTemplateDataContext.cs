using ProjectTools.App.Views.Pages;
using ProjectTools.App.Views.Pages.PrepareTemplatePageControls;

namespace ProjectTools.App.DataContexts;

/// <summary>
///     The data context for the prepare template page.
/// </summary>
public class PrepareTemplateDataContext : BaseObservableDataContext
{
    public PreprocessConfigurationControl ControlPreprocessConfig;

    public SlugConfigurationControl ControlSlugConfig;

    public TemplateConfigurationControl ControlTemplateConfig;
    public PrepareTemplatePage PagePrepareTemplate;

    public PrepareTemplateDataContext(PrepareTemplatePage prepareTemplatePage,
        PreprocessConfigurationControl preprocessConfigurationControl,
        SlugConfigurationControl slugConfigurationControl, TemplateConfigurationControl templateConfigurationControl)
    {
        this.PagePrepareTemplate = prepareTemplatePage;
        this.ControlPreprocessConfig = preprocessConfigurationControl;
        this.ControlSlugConfig = slugConfigurationControl;
        this.ControlTemplateConfig = templateConfigurationControl;
    }
}
