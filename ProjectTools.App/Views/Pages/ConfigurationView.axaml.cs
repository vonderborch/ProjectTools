using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Settings;

namespace ProjectTools.App.Views.Pages;

public partial class ConfigurationView : UserControl
{
    public ConfigurationView()
    {
        InitializeComponent();

        var settings = AbstractSettings.Load();
        if (settings == null)
        {
            settings = new AppSettings();
        }

        this.GitSources.Text = JsonHelpers.SerializeToString(settings.GitSourcesAndAccessTokens);
        this.Repos.Text = JsonHelpers.SerializeToString(settings.RepositoriesAndGitSources);
        this.SecondsBetweenAppUpdateChecks.Text = settings.SecondsBetweenAppUpdateChecks.ToString();
        this.SecondsBetweenTemplateUpdateChecks.Text = settings.SecondsBetweenTemplateUpdateChecks.ToString();

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

        this.LastClUpdate.Text = clUpdate;
        this.LastGuiUpdate.Text = guiUpdate;

        this.LastTemplatesUpdateCheck.Text = settings.LastTemplatesUpdateCheck.ToString();

        this.PythonVersion.Text = settings.PythonVersion;
        this.SettingsFileVersion.Text = settings.SettingsVersion.ToString();
    }

    private async void SaveSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        var settings = AbstractSettings.Load();
        if (settings == null)
        {
            settings = new AppSettings();
        }

        try
        {
            settings.GitSourcesAndAccessTokens =
                JsonHelpers.DeserializeString<Dictionary<string, string>>(this.GitSources.Text ?? string.Empty,
                    raiseError: true) ?? new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            await OkDialogBox.OpenDialogBox(this, "Error",
                $"Error deserializing Git Sources and Access Tokens: {ex.Message}", 300, 150, showCancelButton: false);
            return;
        }

        try
        {
            settings.RepositoriesAndGitSources =
                JsonHelpers.DeserializeString<Dictionary<string, string>>(this.Repos.Text ?? string.Empty,
                    raiseError: true) ?? new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            await OkDialogBox.OpenDialogBox(this, "Error",
                $"Error deserializing Git Repositories and Sources: {ex.Message}", 300, 150, showCancelButton: false);
            return;
        }

        if (int.TryParse(this.SecondsBetweenAppUpdateChecks.Text, out var appUpdateCheckSeconds))
        {
            settings.SecondsBetweenAppUpdateChecks = appUpdateCheckSeconds;
        }
        else
        {
            await OkDialogBox.OpenDialogBox(this, "Error",
                "Error parsing the seconds between app update checks.", 300, 150, showCancelButton: false);
            return;
        }

        if (int.TryParse(this.SecondsBetweenTemplateUpdateChecks.Text, out var templateUpdateCheckSeconds))
        {
            settings.SecondsBetweenTemplateUpdateChecks = templateUpdateCheckSeconds;
        }
        else
        {
            await OkDialogBox.OpenDialogBox(this, "Error",
                "Error parsing the seconds between template update checks.", 300, 150, showCancelButton: false);
            return;
        }

        try
        {
            settings.Save();
        }
        catch (Exception ex)
        {
            await OkDialogBox.OpenDialogBox(this, "Error",
                $"Error saving settings: {ex.Message}", 300, 150, showCancelButton: false);
        }
    }
}
