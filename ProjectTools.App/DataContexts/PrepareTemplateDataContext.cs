#region

using System;
using System.IO;
using Avalonia.Controls;
using ProjectTools.App.DataContexts.PrepareTemplateSubContexts;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.TemplateBuilders;
using ProjectTools.Core.Templates;
using ReactiveUI;

#endregion

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
    ///     Whether the preprocess configuration components are enabled.
    /// </summary>
    private bool _preprocessConfigurationEnabled = true;

    /// <summary>
    ///     Whether the slug configuration components are enabled.
    /// </summary>
    private bool _slugConfigurationEnabled;

    /// <summary>
    ///     Whether the template configuration components are enabled.
    /// </summary>
    private bool _templateConfigurationEnabled;

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
    public AbstractTemplateBuilder? TemplateBuilder;

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

        this.PreprocessDataContext = new PreprocessDataContext(this);
        this.TemplateConfigDataContext = new TemplateConfigurationDataContext(this);
        this.SlugDataContext = new SlugDataContext(this);
    }

    /// <summary>
    ///     The data context for the preprocess configuration control.
    /// </summary>
    public PreprocessDataContext PreprocessDataContext { get; }

    /// <summary>
    ///     The slug data.
    /// </summary>
    public SlugDataContext SlugDataContext { get; }

    /// <summary>
    ///     The template configuration data context.
    /// </summary>
    public TemplateConfigurationDataContext TemplateConfigDataContext { get; }

    /// <summary>
    ///     The template settings file.
    /// </summary>
    public string TemplateSettingsFile =>
        Path.Combine(this.PreprocessDataContext.Directory, TemplateConstants.TemplateSettingsFileName);

    /// <summary>
    ///     Whether the preprocess configuration components are enabled.
    /// </summary>
    public bool PreprocessConfigurationEnabled
    {
        get => this._preprocessConfigurationEnabled;
        set => this.RaiseAndSetIfChanged(ref this._preprocessConfigurationEnabled, value);
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

    private void EnabledSlugConfigurationData()
    {
        this.SlugDataContext.RefreshContext();
    }

    private void EnableTemplateConfigurationData()
    {
        if (this.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        this.TemplateConfigDataContext.EnableContext();
        this.SlugConfigurationEnabled = true;
    }

    /// <summary>
    ///     Resets the slug configuration data.
    /// </summary>
    private void ResetSlugConfigurationData()
    {
        this.SlugDataContext.RefreshContext();
    }

    /// <summary>
    ///     Resets the template configuration data.
    /// </summary>
    private void ResetTemplateConfigurationData()
    {
        this.TemplateConfigDataContext.ClearContext();
        this.SlugConfigurationEnabled = false;
        this.PreparationTemplate = null;
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
}
