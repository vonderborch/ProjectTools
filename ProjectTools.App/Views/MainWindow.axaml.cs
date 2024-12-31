using Avalonia.Controls;
using ProjectTools.App.DataContexts;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.Core.Settings;

namespace ProjectTools.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        PageControlDataContext pageControlDataContext = new();
        pageControlDataContext.ChangeViewAction += this.PanelViews.ChangeView;

        this.PanelControls.DataContext = pageControlDataContext;
        this.PanelViews.DataContext = pageControlDataContext;

        if (AbstractSettings.Load() == null)
        {
            pageControlDataContext.LockedToPage = true;
            this.PanelViews.ChangeView(Page.Settings);
        }
        else
        {
            this.PanelViews.ChangeView(Page.Home);
        }
    }
}
