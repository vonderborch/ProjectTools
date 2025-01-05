using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ProjectTools.App.Dialogs.YesNoDialogBox;

/// <summary>
///     A simple Yes/No Dialog Box
/// </summary>
public partial class YesNoDialogBox : Window
{
    /// <summary>
    ///     The result of the dialog box.
    /// </summary>
    private bool? _result;

    /// <summary>
    ///     Initializes a new instance of the dialog box.
    /// </summary>
    public YesNoDialogBox()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     The action when the No Button is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ButtonNo_OnClick(object? sender, RoutedEventArgs e)
    {
        this._result = false;
        Close(false);
    }

    /// <summary>
    ///     The action when the Yes Button is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ButtonYes_OnClick(object? sender, RoutedEventArgs e)
    {
        this._result = true;
        Close(true);
    }

    /// <summary>
    ///     Opens the dialog box.
    /// </summary>
    /// <param name="parent">The parent of the dialog box.</param>
    /// <param name="title">The title of the dialog box.</param>
    /// <param name="description">The description of the dialog box.</param>
    /// <param name="width">The width of the dialog box.</param>
    /// <param name="height">The height of the dialog box.</param>
    /// <param name="defaultResult">The default result for the dialog box. Defaults to false.</param>
    /// <param name="yesButtonText">The text for the Yes button. Defaults to Yes.</param>
    /// <param name="showYesButton">True to show the Yes button, False otherwise. Defaults to True.</param>
    /// <param name="noButtonText">The text for the No button. Defaults to No.</param>
    /// <param name="showNoButton">True to show the No button, False otherwise. Defaults to True.</param>
    /// <returns>The results of the dialog box. True if Yes was selected, False if No was selected.</returns>
    /// <exception cref="Exception">Raised if we can't find the top level window for the parent control.</exception>
    public static async Task<bool> Open(UserControl parent, string title, string description, int width,
        int height, bool defaultResult = false, string yesButtonText = "Yes", bool showYesButton = true,
        string noButtonText = "No", bool showNoButton = true)
    {
        var window = GetTopLevel(parent) as Window;
        if (window is null)
        {
            throw new Exception("Could not find the top level window!");
        }

        var dialogBox = new YesNoDialogBox
        {
            Width = width,
            Height = height
        };
        dialogBox.TextBlockTitle.Text = title;
        dialogBox.TextBlockDescription.Text = description;
        dialogBox.ButtonYes.Content = yesButtonText;
        dialogBox.ButtonNo.Content = noButtonText;
        dialogBox.ButtonYes.IsVisible = showYesButton;
        dialogBox.ButtonNo.IsVisible = showNoButton;

        await dialogBox.ShowDialog(window);
        return dialogBox._result ?? defaultResult;
    }
}
