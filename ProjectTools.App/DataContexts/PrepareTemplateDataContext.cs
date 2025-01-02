using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.TemplateBuilders;
using ProjectTools.Core.Templates;
using ReactiveUI;

namespace ProjectTools.App.DataContexts;

/// <summary>
///     The data context for the prepare template page.
/// </summary>
public class PrepareTemplateDataContext : ReactiveObject
{
    /// <summary>
    ///     The console log.
    /// </summary>
    private readonly SelectableTextBlock? _consoleLog;

    /// <summary>
    ///     The available template builders.
    /// </summary>
    private List<string> _availableTemplateBuilders;

    /// <summary>
    ///     Whether to force overwrite.
    /// </summary>
    private bool _forceOverwrite;

    /// <summary>
    ///     The output directory.
    /// </summary>
    private string _outputDirectory = string.Empty;

    /// <summary>
    ///     Whether the preprocess configuration components are enabled.
    /// </summary>
    private bool _preprocessConfigurationEnabled = true;

    /// <summary>
    ///     The selected template builder index.
    /// </summary>
    private int _selectedTemplateBuilderIndex;

    /// <summary>
    ///     Whether to skip cleaning.
    /// </summary>
    private bool _skipCleaning;

    /// <summary>
    ///     Whether the slug configuration components are enabled.
    /// </summary>
    private bool _slugConfigurationEnabled;

    /// <summary>
    ///     Whether the template configuration components are enabled.
    /// </summary>
    private bool _templateConfigurationEnabled;

    /// <summary>
    ///     The template directory.
    /// </summary>
    private string _templateDirectory = string.Empty;

    /// <summary>
    ///     Whether to run in WhatIf mode.
    /// </summary>
    private bool _whatIf;

    /// <summary>
    ///     Whether we had a preparation template already or not.
    /// </summary>
    public bool HadTemplate;

    /// <summary>
    ///     The preparation template.
    /// </summary>
    public PreparationTemplate? PreparationTemplate;

    /// <summary>
    ///     The template builder.
    /// </summary>
    public AbstractTemplateBuilder TemplateBuilder;

    /// <summary>
    ///     The Template Preparer.
    /// </summary>
    public Preparer TemplatePreparer;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PrepareTemplateDataContext" /> class.
    /// </summary>
    /// <param name="consoleLog">The console log object.</param>
    public PrepareTemplateDataContext(SelectableTextBlock? consoleLog = null)
    {
        this._consoleLog = consoleLog;
        ClearLog();
        this.TemplatePreparer = new Preparer();
        var availableTemplateBuilders = this.TemplatePreparer.GetTemplateBuilders();
        var options = availableTemplateBuilders.Select(x => x.Name).ToList();
        options.Insert(0, "auto");
        this._availableTemplateBuilders = options;
    }

    /// <summary>
    ///     The template settings file.
    /// </summary>
    public string TemplateSettingsFile =>
        Path.Combine(this.TemplateDirectory, TemplateConstants.TemplateSettingsFileName);

    /// <summary>
    ///     Whether to skip cleaning.
    /// </summary>
    public bool SkipCleaning
    {
        get => this._skipCleaning;
        set => this.RaiseAndSetIfChanged(ref this._skipCleaning, value);
    }

    /// <summary>
    ///     Whether to force overwrite.
    /// </summary>
    public bool ForceOverwrite
    {
        get => this._forceOverwrite;
        set => this.RaiseAndSetIfChanged(ref this._forceOverwrite, value);
    }

    /// <summary>
    ///     Whether to run in WhatIf mode.
    /// </summary>
    public bool WhatIf
    {
        get => this._whatIf;
        set => this.RaiseAndSetIfChanged(ref this._whatIf, value);
    }

    /// <summary>
    ///     The selected template builder.
    /// </summary>
    public string SelectedTemplateBuilder => this.AvailableTemplateBuilders[this.SelectedTemplateBuilderIndex];

    /// <summary>
    ///     The available template builders.
    /// </summary>
    public List<string> AvailableTemplateBuilders
    {
        get => this._availableTemplateBuilders;
        set
        {
            this.RaiseAndSetIfChanged(ref this._availableTemplateBuilders, value);
            this.SelectedTemplateBuilderIndex = 0;
        }
    }

    /// <summary>
    ///     The selected template builder index.
    /// </summary>
    public int SelectedTemplateBuilderIndex
    {
        get => this._selectedTemplateBuilderIndex;
        set => this.RaiseAndSetIfChanged(ref this._selectedTemplateBuilderIndex, value);
    }

    /// <summary>
    ///     Whether the preprocess configuration components are enabled.
    /// </summary>
    public bool PreprocessConfigurationEnabled
    {
        get => this._preprocessConfigurationEnabled;
        set => this.RaiseAndSetIfChanged(ref this._preprocessConfigurationEnabled, value);
    }

    /// <summary>
    ///     Whether the template configuration components are enabled.
    /// </summary>
    public bool TemplateConfigurationEnabled
    {
        get => this._templateConfigurationEnabled;
        set
        {
            this.RaiseAndSetIfChanged(ref this._templateConfigurationEnabled, value);
            if (value)
            {
                EnableTemplateConfigurationData();
            }
            else
            {
                ResetTemplateConfigurationData();
            }
        }
    }

    /// <summary>
    ///     Whether the slug configuration components are enabled.
    /// </summary>
    public bool SlugConfigurationEnabled
    {
        get => this._slugConfigurationEnabled;
        set
        {
            this.RaiseAndSetIfChanged(ref this._slugConfigurationEnabled, value);
            if (value)
            {
                EnabledSlugConfigurationData();
            }
            else
            {
                ResetSlugConfigurationData();
            }
        }
    }

    /// <summary>
    ///     The Template Directory.
    /// </summary>
    public string TemplateDirectory
    {
        get => this._templateDirectory;
        set
        {
            if (string.IsNullOrEmpty(this.OutputDirectory) ||
                Path.GetDirectoryName(this._templateDirectory) == this.OutputDirectory)
            {
                this.OutputDirectory = Path.GetDirectoryName(value);
            }

            this.RaiseAndSetIfChanged(ref this._templateDirectory, value);
        }
    }

    /// <summary>
    ///     The Output Directory.
    /// </summary>
    public string OutputDirectory
    {
        get => this._outputDirectory;
        set => this.RaiseAndSetIfChanged(ref this._outputDirectory, value);
    }

    /// <summary>
    ///     The template name.
    /// </summary>
    public string TemplateName
    {
        get => this.PreparationTemplate?.Name ?? string.Empty;
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.Name, value);
            }
        }
    }

    /// <summary>
    ///     The template version.
    /// </summary>
    public string TemplateVersion
    {
        get => this.PreparationTemplate?.Version ?? string.Empty;
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.Version, value);
            }
        }
    }

    /// <summary>
    ///     The template author.
    /// </summary>
    public string TemplateAuthor
    {
        get => this.PreparationTemplate?.Author ?? string.Empty;
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.Author, value);
            }
        }
    }

    /// <summary>
    ///     The template description.
    /// </summary>
    public string TemplateDescription
    {
        get => this.PreparationTemplate?.Description ?? string.Empty;
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.Description, value);
            }
        }
    }

    /// <summary>
    ///     The template rename only paths.
    /// </summary>
    public string TemplateRenameOnlyPaths
    {
        get
        {
            if (this.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this.PreparationTemplate.RenameOnlyPaths);
        }
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.RenameOnlyPaths,
                    value.Split(Environment.NewLine).ToList());
            }
        }
    }

    /// <summary>
    ///     The template paths to remove.
    /// </summary>
    public string TemplatePathsToRemove
    {
        get
        {
            if (this.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this.PreparationTemplate.PathsToRemove);
        }
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.RenameOnlyPaths,
                    value.Split(Environment.NewLine).ToList());
                this.PreparationTemplate.PathsToRemove = value.Split(Environment.NewLine).ToList();
            }
        }
    }

    /// <summary>
    ///     The template prepare excluded paths.
    /// </summary>
    public string TemplatePrepareExcludedPaths
    {
        get
        {
            if (this.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this.PreparationTemplate.PrepareExcludedPaths);
        }
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.PrepareExcludedPaths,
                    value.Split(Environment.NewLine).ToList());
            }
        }
    }

    /// <summary>
    ///     The template python script paths.
    /// </summary>
    public string TemplatePythonScriptPaths
    {
        get
        {
            if (this.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this.PreparationTemplate.PythonScriptPaths);
        }
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.PythonScriptPaths,
                    value.Split(Environment.NewLine).ToList());
            }
        }
    }

    /// <summary>
    ///     The template instructions.
    /// </summary>
    public string TemplateInstructions
    {
        get
        {
            if (this.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this.PreparationTemplate.Instructions);
        }
        set
        {
            if (this.PreparationTemplate is not null)
            {
                this.RaiseAndSetIfChanged(ref this.PreparationTemplate.Instructions,
                    value.Split(Environment.NewLine).ToList());
            }
        }
    }

    /// <summary>
    ///     Clears the console log.
    /// </summary>
    public void ClearLog()
    {
        if (this._consoleLog == null)
        {
            return;
        }

        this._consoleLog.Text = string.Empty;
    }

    private void EnableTemplateConfigurationData()
    {
        if (this.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        // Template Name
        var temp = this.PreparationTemplate.Name;
        this.TemplateName = string.Empty;
        this.TemplateName = temp;

        // Template Version
        temp = this.PreparationTemplate.Version;
        this.TemplateVersion = string.Empty;
        this.TemplateVersion = temp;

        // Template Author
        temp = this.PreparationTemplate.Author;
        this.TemplateAuthor = string.Empty;
        this.TemplateAuthor = temp;

        // Template Description
        temp = this.PreparationTemplate.Description;
        this.TemplateDescription = string.Empty;
        this.TemplateDescription = temp;

        // Template Rename Only Paths
        temp = string.Join(Environment.NewLine, this.PreparationTemplate.RenameOnlyPaths);
        this.TemplateRenameOnlyPaths = string.Empty;
        this.TemplateRenameOnlyPaths = temp;

        // Template Paths To Remove
        temp = string.Join(Environment.NewLine, this.PreparationTemplate.PathsToRemove);
        this.TemplatePathsToRemove = string.Empty;
        this.TemplatePathsToRemove = temp;

        // Template Prepare Excluded Paths
        temp = string.Join(Environment.NewLine, this.PreparationTemplate.PrepareExcludedPaths);
        this.TemplatePrepareExcludedPaths = string.Empty;
        this.TemplatePrepareExcludedPaths = temp;

        // Template Python Script Paths
        temp = string.Join(Environment.NewLine, this.PreparationTemplate.PythonScriptPaths);
        this.TemplatePythonScriptPaths = string.Empty;
        this.TemplatePythonScriptPaths = temp;

        // Template Instructions
        temp = string.Join(Environment.NewLine, this.PreparationTemplate.Instructions);
        this.TemplateInstructions = string.Empty;
        this.TemplateInstructions = temp;
    }

    private void EnabledSlugConfigurationData()
    {
    }

    /// <summary>
    ///     Writes a message to the log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>Whether we wrote to the console or not.</returns>
    public bool WriteLog(string message)
    {
        if (this._consoleLog == null)
        {
            return false;
        }

        this._consoleLog.Text = $"{this._consoleLog.Text}{Environment.NewLine}{message}";
        return true;
    }

    /// <summary>
    ///     Resets the template configuration data.
    /// </summary>
    private void ResetTemplateConfigurationData()
    {
        this.TemplateName = string.Empty;
        this.TemplateVersion = string.Empty;
        this.TemplateAuthor = string.Empty;
        this.TemplateDescription = string.Empty;
        this.TemplateRenameOnlyPaths = string.Empty;
        this.TemplatePathsToRemove = string.Empty;
        this.TemplatePrepareExcludedPaths = string.Empty;
        this.TemplatePythonScriptPaths = string.Empty;
        this.TemplateInstructions = string.Empty;
        this.SlugConfigurationEnabled = false;

        this.PreparationTemplate = null;
    }

    /// <summary>
    ///     Resets the slug configuration data.
    /// </summary>
    private void ResetSlugConfigurationData()
    {
    }
}
