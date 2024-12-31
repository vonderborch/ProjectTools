using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;

namespace ProjectTools.App.Views;

public partial class ControlPanel : UserControl
{
    public ControlPanel()
    {
        InitializeComponent();
    }

    private ControlPanelViewModel ViewModel => (ControlPanelViewModel)this.DataContext;

    public void ButtonHome_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ChangeView(View.Home);
    }

    public void ButtonHelp_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ChangeView(View.Help);
    }

    public void ButtonConfiguration_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ChangeView(View.Settings);
    }

    public void ButtonGenerateProject_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ChangeView(View.GenerateProject);
    }

    public void ButtonPrepareTemplate_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ChangeView(View.PrepareTemplate);
    }

    public void ButtonExit_Click(object sender, RoutedEventArgs args)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        window?.Close();
    }
}
