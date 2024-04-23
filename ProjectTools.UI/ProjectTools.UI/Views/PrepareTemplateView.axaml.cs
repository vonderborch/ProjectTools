using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ProjectTools.ViewModels;

namespace ProjectTools.Views;

/// <summary>
/// Represents a view for preparing a template.
/// </summary>
public partial class PrepareTemplateView : UserControl
{
    /// <summary>
    /// Represents a view for preparing a template.
    /// </summary>
    public PrepareTemplateView()
    {
        InitializeComponent();
    }
    
    private PrepareTemplateViewModel ViewModel => (PrepareTemplateViewModel)DataContext;
    
    public async void FindTemplateProjectHandler(object sender, RoutedEventArgs args)
    {
        var directory = await GetDirectory("Open Project Folder");
        if (!string.IsNullOrEmpty(directory))
        {
            ViewModel.Directory = directory;
        }
    }
    
    public async void SaveFileLocationHandler(object sender, RoutedEventArgs args)
    {
        var directory = await GetDirectory("Select Output Directory");
        if (!string.IsNullOrEmpty(directory))
        {
            ViewModel.OutputDirectory = directory;
        }
    }
    
    public void GenerateTemplateHandler(object sender, RoutedEventArgs args)
    {
    }
    
    public void ResetHandler(object sender, RoutedEventArgs args)
    {
    }

    private async Task<string> GetDirectory(string title)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel != null)
        {
            // Start async operation to open the dialog.
            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = title,
                AllowMultiple = false
            });

            if (folders.Count >= 1)
            {
                return folders[0].Path.AbsolutePath;
            }
        }

        return string.Empty;
    }
}