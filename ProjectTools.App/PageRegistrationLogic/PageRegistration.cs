using System;

namespace ProjectTools.App.PageRegistrationLogic;

/// <summary>
///     An attribute that registers a page for use in the application.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PageRegistration : Attribute
{
    /// <summary>
    ///     Registers the page for use in the application.
    /// </summary>
    /// <param name="displayName">The display name.</param>
    /// <param name="page">The page.</param>
    public PageRegistration(string displayName, Page page)
    {
        this.DisplayName = displayName;
        this.Page = page;
    }

    /// <summary>
    ///     The display name for the page.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    ///     The page.
    /// </summary>
    public Page Page { get; }
}
