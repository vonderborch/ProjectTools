using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectTools.Core;

namespace ProjectTools.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public Settings Settings;
    
    public SettingsViewModel()
    {
        Settings? settings = null;
        if (File.Exists(Constants.SettingsFile))
        {
            settings = Settings.LoadFile(Constants.SettingsFile);
        }

        settings ??= new Settings();
        Settings = settings;
    }

    public string GitWebPath
    {
        get => Settings.GitWebPath;
        set => Settings.GitWebPath = value;
    }
    
    public string GitAccessToken
    {
        get => Settings.GitAccessToken;
        set => Settings.GitAccessToken = value;
    }

    public string GitTemplateRepositoriesText
    {
        get => string.Join(Environment.NewLine, GitTemplateRepositories);
        set => GitTemplateRepositories = value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
    
    public List<string> GitTemplateRepositories
    {
        get => Settings.TemplateRepositories;
        set => Settings.TemplateRepositories = new(value);
    }
}