using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using ProjectTools.App.DataContexts;
using ProjectTools.App.PageRegistrationLogic;

namespace ProjectTools.App.Views;

public partial class ControlPanel : UserControl
{
    public ControlPanel()
    {
        InitializeComponent();

        var pages = PageRegistry.GetPages();
        foreach (var page in pages)
        {
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

        var exitButton = new Button
        {
            Content = "Exit",
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        exitButton.Click += ButtonExit_Click;
        this.DockPanelControls.Children.Add(exitButton);
    }

    private PageControlDataContext Context => (PageControlDataContext)this.DataContext;

    public void ButtonExit_Click(object sender, RoutedEventArgs args)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        window?.Close();
    }
}
