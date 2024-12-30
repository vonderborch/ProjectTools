using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;

namespace ProjectTools.App.Views;

public partial class OkDialogBox : Window
{
    public OkDialogBox()
    {
        InitializeComponent();
    }

    private OkDialogBoxViewModel ViewModel => (OkDialogBoxViewModel)this.DataContext;

    public void ButtonOk_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ResultIsOk = true;
        Close(true);
    }

    public void ButtonCancel_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ResultIsOk = false;
        Close(false);
    }
}
