using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectTools.Core;

namespace ProjectTools.ViewModels;

/// <summary>
/// Represents the view model for the settings view.
/// </summary>
public class SettingsViewModel : ViewModelBase
{
    /// <summary>
    /// Represents the settings for the application.
    /// </summary>
    public Settings Settings;

    /// <summary>
    /// Represents the view model for the settings view.
    /// </summary>
    public SettingsViewModel()
    {
        Settings? settings = null;
        if (File.Exists(Constants.SettingsFile)) settings = Settings.LoadFile(Constants.SettingsFile);

        settings ??= new Settings();
        Settings = settings;
    }

    /// <summary>
    /// Gets or sets the Git web path.
    /// </summary>
    public string GitWebPath
    {
        get => Settings.GitWebPath;
        set => Settings.GitWebPath = value;
    }

    /// <summary>
    /// Represents the access token used for Git authentication.
    /// </summary>
    public string GitAccessToken
    {
        get => Settings.GitAccessToken;
        set => Settings.GitAccessToken = value;
    }

    /// <summary>
    /// Gets or sets the Git template repositories as a newline-separated string.
    /// </summary>
    /// <remarks>
    /// This property is used to bind the template repositories TextBox in the SettingsView.
    /// </remarks>
    public string GitTemplateRepositoriesText
    {
        get => string.Join(Environment.NewLine, GitTemplateRepositories);
        set => GitTemplateRepositories =
            value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    /// <summary>
    /// Gets or sets the list of repositories templates are pulled from.
    /// </summary>
    public List<string> GitTemplateRepositories
    {
        get => Settings.TemplateRepositories;
        set => Settings.TemplateRepositories = new List<string>(value);
    }
}