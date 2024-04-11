using ProjectTools.Core;
using ReactiveUI;

namespace ProjectTools.ViewModels;

public class MainViewModel : ViewModelBase
{
    // The default is the first page
    private ViewModelBase _currentViewModel;

    private Pages _currentPage;

    private string _currentPageName;

    public bool ControlsLocked;
    
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
    
    
    public string CurrentPageName
    {
        get => _currentPageName;
        set => this.RaiseAndSetIfChanged(ref _currentPageName, value);
    }

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