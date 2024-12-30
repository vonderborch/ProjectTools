using System;

namespace ProjectTools.App.ViewModels;

public class ControlPanelViewModel : ViewModelBase
{
    public Action<View>? ChangeViewAction;

    public View CurrentView { get; set; } = View.Home;

    public void ChangeView(View newView)
    {
        if (this.CurrentView != newView)
        {
            if (this.ChangeViewAction is not null)
            {
                this.ChangeViewAction(newView);
            }

            this.CurrentView = newView;
        }
    }
}
