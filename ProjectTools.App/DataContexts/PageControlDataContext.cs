using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectTools.App.PageRegistrationLogic;

namespace ProjectTools.App.DataContexts;

/// <summary>
///     The data context for the page control.
/// </summary>
public class PageControlDataContext : ObservableObject
{
    public Action<Page>? ChangeViewAction;

    public Page CurrentPage { get; set; } = Page.Home;

    public bool LockedToPage { get; set; } = false;

    public void ChangeView(Page newPage)
    {
        if (this.CurrentPage != newPage && !this.LockedToPage)
        {
            if (this.ChangeViewAction is not null)
            {
                this.ChangeViewAction(newPage);
            }

            this.CurrentPage = newPage;
        }
    }
}
