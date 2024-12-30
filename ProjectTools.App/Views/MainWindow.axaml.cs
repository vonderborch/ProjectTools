using Avalonia.Controls;
using ProjectTools.App.ViewModels;
using ProjectTools.Core.Settings;

namespace ProjectTools.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ControlPanelViewModel controlPanelContext = new();
        ViewControlViewModel viewControlContext = new();

        controlPanelContext.ChangeViewAction += viewControlContext.ChangeView;
        controlPanelContext.ChangeViewAction += this.Views.ChangeView;

        this.Controls.DataContext = controlPanelContext;
        this.Views.DataContext = viewControlContext;

        if (AbstractSettings.Load() == null)
        {
            controlPanelContext.LockedToPage = true;
            this.Views.ChangeView(View.Settings);
        }
        else
        {
            this.Views.ChangeView(View.Home);
        }
    }
}
