using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectTools.App.PageRegistrationLogic;

/// <summary>
///     A registry for all pages in the application.
/// </summary>
public static class PageRegistry
{
    /// <summary>
    ///     The page name to page enum map.
    /// </summary>
    private static readonly Dictionary<string, Page> _pageNameToEnum;

    /// <summary>
    ///     The page enum to page registration and type enum.
    /// </summary>
    private static readonly Dictionary<Page, (PageRegistration, Type)> _pageCache;

    /// <summary>
    ///     Initializes the Page Registry.
    /// </summary>
    /// <exception cref="Exception">Raised if we encountered a bad page registration.</exception>
    static PageRegistry()
    {
        _pageNameToEnum = new Dictionary<string, Page>();
        _pageCache = new Dictionary<Page, (PageRegistration, Type)>();

        var assembly = Assembly.GetEntryAssembly();
        var types = assembly.GetTypes();

        var pages = types.Where(t => t.GetCustomAttribute<PageRegistration>() != null).ToList();
        foreach (var page in pages)
        {
            var pageRegistration = (PageRegistration?)page.GetCustomAttribute(typeof(PageRegistration), false);
            if (pageRegistration is null)
            {
                throw new Exception("Bad page registration!");
            }

            _pageNameToEnum.Add(pageRegistration.DisplayName, pageRegistration.Page);
            _pageCache.Add(pageRegistration.Page, (pageRegistration, page));
        }
    }

    /// <summary>
    ///     Gets the sorted pages.
    /// </summary>
    /// <returns>The pages.</returns>
    public static List<PageRegistration> GetPages()
    {
        var pages = _pageCache.Values
            .Select(x => x.Item1).OrderBy(x => (int)x.Page).ToList();
        return pages;
    }

    /// <summary>
    ///     Gets the type for the specified page.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <returns>The type.</returns>
    public static Type GetTypeForPage(Page page)
    {
        return _pageCache[page].Item2;
    }
}
