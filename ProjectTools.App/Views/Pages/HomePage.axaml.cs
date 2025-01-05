using Avalonia.Controls;
using ProjectTools.App.PageRegistrationLogic;

namespace ProjectTools.App.Views.Pages;

/// <summary>
///     The home page.
/// </summary>
[PageRegistration("Home", Page.Home)]
public partial class HomePage : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HomePage" /> class.
    /// </summary>
    public HomePage()
    {
        InitializeComponent();
    }
}
