using System.Web;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.App.Views.Pages;

/// <summary>
///     The report issue page.
/// </summary>
[PageRegistration("Report Issue", Page.ReportIssue)]
public partial class ReportIssuePage : UserControl
{
    public ReportIssuePage()
    {
        InitializeComponent();
    }

    private void ButtonSubmit_OnClick(object? sender, RoutedEventArgs e)
    {
        // Construct Urls
        var title = $"BUG: {this.TextBoxTitle.Text}";
        title = HttpUtility.UrlEncode(title);
        var description = HttpUtility.UrlEncode(this.TextBoxDescription.Text);

        var baseUrl = $"{AppConstants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=bug";

        UrlHelpers.OpenUrl(url, $"Please go to {baseUrl} to file a bug!");
    }
}