using System;
using System.Threading.Tasks;
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

    private void ButtonOk_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ResultIsOk = true;
        Close(true);
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs args)
    {
        this.ViewModel.ResultIsOk = false;
        Close(false);
    }

    public static async Task<bool> OpenDialogBox(UserControl control, string title, string description, int width,
        int height,
        string okButtonText = "OK", string cancelButtonText = "Cancel", bool defaultResult = false,
        bool showCancelButton = true, bool showOkButton = true)
    {
        var window = GetTopLevel(control) as Window;

        if (window is null)
        {
            throw new Exception("Could not find the top level window!");
        }

        var okDialogBoxViewModel = new OkDialogBoxViewModel(title, description)
        {
            CancelText = cancelButtonText,
            OkText = okButtonText
        };
        var okDialogBox = new OkDialogBox
        {
            DataContext = okDialogBoxViewModel,
            Width = width,
            Height = height
        };
        okDialogBox.CancelButton.IsEnabled = showCancelButton;
        okDialogBox.OkButton.IsEnabled = showOkButton;
        okDialogBox.CancelButton.IsVisible = showCancelButton;
        okDialogBox.OkButton.IsVisible = showOkButton;

        await okDialogBox.ShowDialog(window);
        return okDialogBoxViewModel.ResultIsOk ?? defaultResult;
    }
}
