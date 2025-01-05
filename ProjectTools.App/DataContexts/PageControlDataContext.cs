#region

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectTools.App.PageRegistrationLogic;

#endregion

namespace ProjectTools.App.DataContexts;

/// <summary>
///     The data context for the page control.
/// </summary>
public class PageControlDataContext : ObservableObject
{
    /// <summary>
    ///     Any actions to perform when the view changes.
    /// </summary>
    public Action<Page>? ChangeViewAction;

    /// <summary>
    ///     The current page.
    /// </summary>
    public Page CurrentPage { get; set; } = Page.Home;

    /// <summary>
    ///     Whether the view is locked to the current page.
    /// </summary>
    public bool LockedToPage { get; set; } = false;

    /// <summary>
    ///     Changes the view to the specified page.
    /// </summary>
    /// <param name="newPage"></param>
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
