using System.Web;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.App.Views.Pages;

/// <summary>
///     The make suggestion page.
/// </summary>
[PageRegistration("Make Suggestion", Page.MakeSuggestion)]
public partial class MakeSuggestionPage : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MakeSuggestionPage" /> class.
    /// </summary>
    public MakeSuggestionPage()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Submits the suggestion.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The args.</param>
    private void ButtonSubmit_OnClick(object? sender, RoutedEventArgs e)
    {
        // Construct Urls
        var title = $"FEATURE: {this.TextBoxTitle.Text}";
        title = HttpUtility.UrlEncode(title);
        var description = HttpUtility.UrlEncode(this.TextBoxDescription.Text);

        var baseUrl = $"{AppConstants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=enhancement";

        UrlHelpers.OpenUrl(url, $"Please go to {baseUrl} to make a suggestion!");
    }
}
