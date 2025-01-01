using ProjectTools.App.Views.Pages;
using ProjectTools.App.Views.Pages.PrepareTemplatePageControls;
using ReactiveUI;

namespace ProjectTools.App.DataContexts;

/// <summary>
///     The data context for the prepare template page.
/// </summary>
public class PrepareTemplateDataContext : ReactiveObject
{
    private string _outputDirectory = string.Empty;
    private string _templateDirectory = string.Empty;

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

    public string TemplateDirectory
    {
        get => this._templateDirectory;
        set => this.RaiseAndSetIfChanged(ref this._templateDirectory, value);
    }

    public string OutputDirectory
    {
        get => this._outputDirectory;
        set => this.RaiseAndSetIfChanged(ref this._outputDirectory, value);
    }
}
