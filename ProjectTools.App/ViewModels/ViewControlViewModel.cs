namespace ProjectTools.App.ViewModels;

public class ViewControlViewModel
{
    public View CurrentView { get; set; } = View.Home;

    public void ChangeView(View newView)
    {
        this.CurrentView = newView;
    }
}
