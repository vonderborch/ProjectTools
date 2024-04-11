using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectTools.Core;
using ProjectTools.ViewModels;

namespace ProjectTools.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        //GitWebPathTextBox.Text = ViewModel.GitWebPath;
        //GitAccessTokenTextBox.Text = ViewModel.GitAccessToken;
        //TemplateRepositoriesTextBox.Text = string.Join(Environment.NewLine, ViewModel.GitTemplateRepositories);
    }
    
    private SettingsViewModel ViewModel => (SettingsViewModel)DataContext;
    
    public void SaveSettingsClickHandler(object sender, RoutedEventArgs args)
    {
        ViewModel.GitWebPath = GitWebPathTextBox.Text;
        ViewModel.GitAccessToken = GitAccessTokenTextBox.Text;
        ViewModel.GitTemplateRepositories = TemplateRepositoriesTextBox.Text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        ViewModel.Settings.SaveFile(Constants.SettingsFile);
        MainViewModel vm = (MainViewModel)Parent.DataContext;

        vm.ControlsLocked = false;
    }
}