using System.Linq;
using Avalonia.Controls;
using ProjectTools.App.ViewModels;
using ProjectTools.App.Views.Pages.PrepareTemplateControls;

namespace ProjectTools.App.Views.Pages;

public partial class PrepareTemplate : UserControl
{
    private readonly PrepareTemplateViewModel _prepareViewModel;

    public PrepareTemplate()
    {
        InitializeComponent();

        PreprocessConfiguration preprocessConfig = new();
        TemplateConfiguration templateConfiguration = new();
        SlugConfiguration slugConfiguration = new();

        this._prepareViewModel =
            new PrepareTemplateViewModel(this, preprocessConfig, templateConfiguration, slugConfiguration);

        preprocessConfig.PrepareViewModel = this._prepareViewModel;
        templateConfiguration.PrepareViewModel = this._prepareViewModel;
        slugConfiguration.PrepareViewModel = this._prepareViewModel;


        var availableTemplateBuilders = this._prepareViewModel.Preparer.GetTemplateBuilders();
        var options = availableTemplateBuilders.Select(x => x.Name).ToList();
        options.Insert(0, "auto");
        preprocessConfig.TemplateBuilders.ItemsSource = options;
        preprocessConfig.TemplateBuilders.SelectedItem = "auto";

        this.ViewerPreprocessConfig.Content = preprocessConfig;
        this.ViewerTemplateConfig.Content = templateConfiguration;
        this.ViewerSlugConfig.Content = slugConfiguration;
    }
}
