using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.ViewModels;

namespace ProjectTools.App.Views.Pages.PrepareTemplateControls;

public partial class SlugConfiguration : UserControl
{
    public PrepareTemplateViewModel PrepareViewModel;

    public SlugConfiguration()
    {
        InitializeComponent();
    }

    private void Slugs_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonAddSlug_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ButtonGenerateTemplate_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
