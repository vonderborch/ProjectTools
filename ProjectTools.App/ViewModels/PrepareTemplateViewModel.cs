using ProjectTools.App.Views.Pages;
using ProjectTools.App.Views.Pages.PrepareTemplateControls;
using ProjectTools.Core;
using ProjectTools.Core.TemplateBuilders;
using ProjectTools.Core.Templates;

namespace ProjectTools.App.ViewModels;

public class PrepareTemplateViewModel : ViewModelBase
{
    public Preparer Preparer;

    public PrepareTemplate PrepareTemplate;

    public PreprocessConfiguration PreprocessConfiguration;

    public SlugConfiguration SlugConfiguration;

    public TemplateConfiguration TemplateConfiguration;

    public PrepareTemplateViewModel(PrepareTemplate prepareTemplate, PreprocessConfiguration preprocessConfiguration,
        TemplateConfiguration templateConfiguration, SlugConfiguration slugConfiguration)
    {
        this.Preparer = new Preparer();
        this.PrepareTemplate = prepareTemplate;
        this.PreprocessConfiguration = preprocessConfiguration;
        this.TemplateConfiguration = templateConfiguration;
        this.SlugConfiguration = slugConfiguration;
    }

    public string TemplateBuilderName { get; set; }

    public AbstractTemplateBuilder TemplateBuilder { get; set; }

    public string OutputDirectory { get; set; }

    public string TemplateDirectory { get; set; }

    public bool SkipCleaning { get; set; }

    public bool ForceOverride { get; set; }

    public bool WhatIf { get; set; }

    public PreparationTemplate? PreparationTemplate { get; set; }

    public bool HadPreparationTemplate { get; set; }
}
