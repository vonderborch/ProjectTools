using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.App.DataContexts;
using ProjectTools.App.Dialogs.YesNoDialogBox;
using ProjectTools.App.PageRegistrationLogic;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Settings;

namespace ProjectTools.App.Views.Pages;

[PageRegistration("Settings", Page.Settings, 30)]
public partial class ConfigurationPage : UserControl
{
    public ConfigurationPage()
    {
        InitializeComponent();

        var settings = AbstractSettings.Load() ?? new AppSettings();

        this.TextBoxGitSources.Text = JsonHelpers.SerializeToString(settings.GitSourcesAndAccessTokens);
        this.TextBoxRepos.Text = JsonHelpers.SerializeToString(settings.RepositoriesAndGitSources);
        this.TextBoxSecondsBetweenAppUpdateChecks.Text = settings.SecondsBetweenAppUpdateChecks.ToString();
        this.TextBoxSecondsBetweenTemplateUpdateChecks.Text = settings.SecondsBetweenTemplateUpdateChecks.ToString();

        var clUpdate = DateTime.MinValue.ToString();
        if (settings.LastAppUpdateCheck.TryGetValue(AppConstants.AppNameCommandLine, out var clUpdateValue))
        {
            clUpdate = clUpdateValue.ToString();
        }

        var guiUpdate = DateTime.MinValue.ToString();
        if (settings.LastAppUpdateCheck.TryGetValue(AppConstants.AppNameGui, out var guiUpdateValue))
        {
            guiUpdate = guiUpdateValue.ToString();
        }

        this.TextBlockLastClUpdate.Text = clUpdate;
        this.TextBlockLastGuiUpdate.Text = guiUpdate;

        this.TextBlockLastTemplatesUpdateCheck.Text = settings.LastTemplatesUpdateCheck.ToString();

        this.TextBlockPythonVersion.Text = settings.PythonVersion;
        this.TextBlockSettingsFileVersion.Text = settings.SettingsVersion.ToString();
    }

    private PageControlDataContext Context => (PageControlDataContext)this.DataContext;

    private async void ButtonSaveSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        var settings = AbstractSettings.Load() ?? new AppSettings();
        try
        {
            settings.GitSourcesAndAccessTokens =
                JsonHelpers.DeserializeString<Dictionary<string, string>>(this.TextBoxGitSources.Text ?? string.Empty,
                    raiseError: true) ?? new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            await YesNoDialogBox.Open(
                this,
                "Error",
                $"Error deserializing Git Sources and Access Tokens: {ex.Message}",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
            return;
        }

        try
        {
            settings.RepositoriesAndGitSources =
                JsonHelpers.DeserializeString<Dictionary<string, string>>(this.TextBoxRepos.Text ?? string.Empty,
                    raiseError: true) ?? new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            await YesNoDialogBox.Open(
                this,
                "Error",
                $"Error deserializing Git Repositories and Sources: {ex.Message}",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
            return;
        }

        if (int.TryParse(this.TextBoxSecondsBetweenAppUpdateChecks.Text, out var appUpdateCheckSeconds))
        {
            settings.SecondsBetweenAppUpdateChecks = appUpdateCheckSeconds;
        }
        else
        {
            await YesNoDialogBox.Open(
                this,
                "Error",
                "Error parsing the seconds between app update checks",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
            return;
        }

        if (int.TryParse(this.TextBoxSecondsBetweenTemplateUpdateChecks.Text, out var templateUpdateCheckSeconds))
        {
            settings.SecondsBetweenTemplateUpdateChecks = templateUpdateCheckSeconds;
        }
        else
        {
            await YesNoDialogBox.Open(
                this,
                "Error",
                "Error parsing the seconds between template update checks.",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
            return;
        }

        try
        {
            settings.Save();
            this.Context.LockedToPage = false;
        }
        catch (Exception ex)
        {
            await YesNoDialogBox.Open(
                this,
                "Error",
                $"Error saving settings: {ex.Message}",
                300,
                150,
                yesButtonText: "OK",
                showNoButton: false
            );
        }
    }
}
