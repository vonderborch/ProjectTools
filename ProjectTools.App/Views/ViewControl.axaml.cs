#region

using System;
using Avalonia.Controls;
using ProjectTools.App.Dialogs.YesNoDialogBox;
using ProjectTools.App.PageRegistrationLogic;

#endregion

namespace ProjectTools.App.Views;

/// <summary>
///     The view control.
/// </summary>
public partial class ViewControl : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ViewControl" /> class.
    /// </summary>
    public ViewControl()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Changes the displayed content.
    /// </summary>
    /// <param name="newPage">The new page to display.</param>
    /// <exception cref="InvalidOperationException">Raised if we could not create the new page.</exception>
    public async void ChangeView(Page newPage)
    {
        try
        {
            var pageType = PageRegistry.GetTypeForPage(newPage);
            var newControl = (UserControl?)Activator.CreateInstance(pageType);
            if (newControl is null)
            {
                throw new InvalidOperationException($"Could not create a control for page {newPage}");
            }

            newControl.DataContext = this.DataContext;
            this.Viewer.Content = newControl;
        }
        catch (Exception ex)
        {
            ;
            await YesNoDialogBox.Open(
                this,
                "Error",
                $"Error While Changing Pages: {ex.Message}",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
        }
    }
}
