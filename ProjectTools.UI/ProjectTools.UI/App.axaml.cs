using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ProjectTools.ViewModels;
using ProjectTools.Views;
using MainWindow = ProjectTools.Views.MainWindow;

namespace ProjectTools;

/// <summary>
/// Represents the entry point of the application.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the application.
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// This method is called when the framework initialization is completed.
    /// It sets the main window of the application based on the platform.
    /// </summary>
    /// <remarks>
    /// If the application is running on a desktop platform,
    /// the main window will be set to an instance of MainWindow with a MainViewModel as its DataContext.
    /// If the application is running on a single view platform,
    /// the main view will be set to an instance of MainView with a MainViewModel as its DataContext.
    /// </remarks>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };

        base.OnFrameworkInitializationCompleted();
    }
}