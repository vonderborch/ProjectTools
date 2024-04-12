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

/// <summary>
/// Represents a view for the settings of the project.
/// </summary>
public partial class SettingsView : UserControl
{
    /// <summary>
    /// Represents a view for the settings of the project.
    /// </summary>
    public SettingsView()
    {
        InitializeComponent();
        //GitWebPathTextBox.Text = ViewModel.GitWebPath;
        //GitAccessTokenTextBox.Text = ViewModel.GitAccessToken;
        //TemplateRepositoriesTextBox.Text = string.Join(Environment.NewLine, ViewModel.GitTemplateRepositories);
    }

    /// <summary>
    /// Represents a view for the settings of the project.
    /// </summary>
    private SettingsViewModel ViewModel => (SettingsViewModel)DataContext;

    /// <summary>
    /// Handles the click event of the Save Settings button in the SettingsView.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="args">The event arguments.</param>
    public void SaveSettingsClickHandler(object sender, RoutedEventArgs args)
    {
        ViewModel.GitWebPath = GitWebPathTextBox.Text;
        ViewModel.GitAccessToken = GitAccessTokenTextBox.Text;
        ViewModel.GitTemplateRepositories = TemplateRepositoriesTextBox.Text
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        ViewModel.Settings.SaveFile(Constants.SettingsFile);
        var vm = (MainViewModel)Parent.DataContext;

        vm.ControlsLocked = false;
    }
}