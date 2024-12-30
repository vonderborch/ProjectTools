using System.Web;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;

namespace ProjectTools.App.Views.Pages.HelpPages;

public partial class ReportIssue : UserControl
{
    public ReportIssue()
    {
        InitializeComponent();
    }

    public void ButtonClickSubmit_Click(object sender, RoutedEventArgs args)
    {
        // Construct Urls
        var title = $"BUG: {this.TitleText.Text}";
        title = HttpUtility.UrlEncode(title);
        var description = HttpUtility.UrlEncode(this.DescriptionText.Text);

        var baseUrl = $"{AppConstants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=bug";

        UrlHelpers.OpenUrl(url, $"Please go to {baseUrl} to file a bug!");
    }
}
