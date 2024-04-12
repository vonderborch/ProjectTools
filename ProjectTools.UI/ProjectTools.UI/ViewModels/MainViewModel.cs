using ProjectTools.Core;
using ReactiveUI;

namespace ProjectTools.ViewModels;

/// <summary>
/// Represents the main view model of the application.
/// </summary>
public class MainViewModel : ViewModelBase
{
    /// <summary>
    /// Represents the current view model in the application.
    /// </summary>
    private ViewModelBase _currentViewModel;

    /// <summary>
    /// Represents the current page in the application.
    /// </summary>
    private Pages _currentPage;

    /// <summary>
    /// Represents the name of the current page in the application.
    /// </summary>
    private string _currentPageName;

    /// <summary>
    /// Represents the current state of whether the controls are locked or not.
    /// </summary>
    public bool ControlsLocked;

    /// <summary>
    /// Represents the main view model of the application.
    /// </summary>
    public MainViewModel()
    {
        if (!Manager.Instance.ValidateSettings())
        {
            ControlsLocked = true;
            _currentPage = Pages.Settings;
            _currentPageName = _currentPage.ToString();
            CurrentPage = new SettingsViewModel();
        }
        else
        {
            ControlsLocked = false;
            _currentPage = Pages.About;
            _currentPageName = _currentPage.ToString();
            CurrentPage = new AboutViewModel();
        }
    }

    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public ViewModelBase CurrentPage
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }


    /// <summary>
    /// Represents the name of the current page in the application.
    /// </summary>
    public string CurrentPageName
    {
        get => _currentPageName;
        set => this.RaiseAndSetIfChanged(ref _currentPageName, value);
    }

    /// <summary>
    /// Changes the current page of the application to the specified page.
    /// </summary>
    /// <param name="newPage">The page to navigate to.</param>
    public void ChangePage(Pages newPage)
    {
        if (newPage != _currentPage && !ControlsLocked)
        {
            CurrentPageName = newPage.ToString();
            _currentPage = newPage;
            switch (newPage)
            {
                case Pages.About:
                    CurrentPage = new AboutViewModel();
                    break;
                case Pages.Settings:
                    CurrentPage = new SettingsViewModel();
                    break;
                case Pages.PrepareTemplate:
                    CurrentPage = new PrepareTemplateViewModel();
                    break;
                case Pages.GenerateProject:
                    CurrentPage = new GenerateProjectViewModel();
                    break;
                case Pages.ReportIssue:
                    CurrentPage = new ReportIssueViewModel();
                    break;
                case Pages.MakeSuggestion:
                    CurrentPage = new MakeSuggestionViewModel();
                    break;
            }
        }
    }
}