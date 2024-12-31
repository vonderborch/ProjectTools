using ProjectTools.App.PageRegistrationLogic;

namespace ProjectTools.App.ViewModels;

public class ViewControlViewModel
{
    public Page CurrentPage { get; set; } = Page.Home;

    public void ChangeView(Page newPage)
    {
        this.CurrentPage = newPage;
    }
}
