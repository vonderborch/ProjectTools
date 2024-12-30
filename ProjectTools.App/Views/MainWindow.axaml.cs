using Avalonia.Controls;
using ProjectTools.App.ViewModels;

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

        this.Views.ChangeView(View.Home);
    }
}
