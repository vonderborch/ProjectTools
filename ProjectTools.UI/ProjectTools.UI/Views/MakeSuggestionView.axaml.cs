using System;
using System.Diagnostics;
using System.Web;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectTools.Core;

namespace ProjectTools.Views;

public partial class MakeSuggestionView : UserControl
{
    public MakeSuggestionView()
    {
        InitializeComponent();
    }
    public void CreateSuggestionClickHandler(object sender, RoutedEventArgs args)
    {
        var title = TitleTextBox.Text;
        var description = DescriptionTextBox.Text;
        
        title = $"FEATURE: {title}";
        title = HttpUtility.UrlEncode(title);
        description = HttpUtility.UrlEncode(description);

        var baseUrl = $"{Constants.ApplicationRepositoryUrl}/issues/new";
        var url = $"{baseUrl}?title={title}&body={description}&labels=enhancement";
        
        try
        {
            var ps = new ProcessStartInfo(url) { UseShellExecute = true, Verb = "open" };
            _ = Process.Start(ps);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}