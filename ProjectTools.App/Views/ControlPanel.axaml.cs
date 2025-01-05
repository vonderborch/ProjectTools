#region

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using ProjectTools.App.DataContexts;
using ProjectTools.App.Helpers;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

#endregion

namespace ProjectTools.App.Views;

/// <summary>
///     The control panel.
/// </summary>
public partial class ControlPanel : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ControlPanel" /> class.
    /// </summary>
    public ControlPanel()
    {
        InitializeComponent();

        var pages = PageRegistry.GetPages();
        for (var i = 0; i < pages.Count; i++)
        {
            var page = pages[i];
            if (page.Page == Page.MakeSuggestion)
            {
                var docsButton = new Button
                {
                    Content = "Documentation",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                docsButton.Click += ButtonDocumentation_Click;
                docsButton.IsEnabled = true;
                this.DockPanelControls.Children.Add(docsButton);
            }

            var button = new Button
            {
                Content = page.DisplayName,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            button.Click += (sender, args) => this.Context.ChangeView(page.Page);
            button.IsEnabled = true;
            this.DockPanelControls.Children.Add(button);
        }

        var updateButton = new Button
        {
            Content = "Check for Updates",
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        updateButton.Click += ButtonUpdateCheck_Click;
        updateButton.IsEnabled = true;
        this.DockPanelControls.Children.Add(updateButton);

        var exitButton = new Button
        {
            Content = "Exit",
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        exitButton.Click += ButtonExit_Click;
        exitButton.IsEnabled = true;
        this.DockPanelControls.Children.Add(exitButton);
    }

    /// <summary>
    ///     The data context.
    /// </summary>
    private PageControlDataContext Context => (PageControlDataContext)this.DataContext;

    /// <summary>
    ///     Opens the documentation for the application.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The args.</param>
    public void ButtonDocumentation_Click(object sender, RoutedEventArgs args)
    {
        var url = $"{AppConstants.ApplicationRepositoryUrl}/wiki";

        UrlHelpers.OpenUrl(url, $"Please go to {url} to file a bug!");
    }

    /// <summary>
    ///     Closes the program.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The args.</param>
    public void ButtonExit_Click(object sender, RoutedEventArgs args)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        window?.Close();
    }

    /// <summary>
    ///     Checks for updates to the application.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The args.</param>
    public void ButtonUpdateCheck_Click(object sender, RoutedEventArgs args)
    {
        UpdateHelper.CheckForUpdates(this, true);
    }
}
