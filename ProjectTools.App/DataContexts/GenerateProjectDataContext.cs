#region

using System;
using Avalonia.Controls;
using ProjectTools.App.DataContexts.GenerateProjectSubContexts;
using ProjectTools.Core.Templates;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts;

public class GenerateProjectDataContext : ReactiveObject
{
    /// <summary>
    ///     The core log.
    /// </summary>
    private readonly SelectableTextBlock? _coreLog;

    /// <summary>
    ///     The instructions log.
    /// </summary>
    private readonly SelectableTextBlock? _instructionsLog;

    /// <summary>
    ///     The script log.
    /// </summary>
    private readonly SelectableTextBlock? _scriptLog;

    /// <summary>
    ///     Whether the core configuration is enabled.
    /// </summary>
    private bool _coreConfigurationEnabled;

    /// <summary>
    ///     Whether the slug configuration is enabled.
    /// </summary>
    private bool _slugConfigurationEnabled;

    /// <summary>
    ///     The selected template's info.
    /// </summary>
    public LocalTemplateInfo? TemplateInfo;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GenerateProjectDataContext" /> class.
    /// </summary>
    /// <param name="coreLog">The core log.</param>
    /// <param name="scriptLog">The script log.</param>
    /// <param name="instructionsLog">The instructions log.</param>
    public GenerateProjectDataContext(SelectableTextBlock? coreLog = null, SelectableTextBlock? scriptLog = null,
        SelectableTextBlock? instructionsLog = null)
    {
        this._coreLog = coreLog;
        this._scriptLog = scriptLog;
        this._instructionsLog = instructionsLog;
        this.TemplateSelectionContext = new TemplateSelectionDataContext(this);
        this.CoreConfigurationContext = new CoreConfigurationDataContext(this);
        this.SlugConfigurationContext = new SlugConfigurationDataContext(this);
    }

    /// <summary>
    ///     The core configuration context.
    /// </summary>
    public CoreConfigurationDataContext CoreConfigurationContext { get; }

    /// <summary>
    ///     The slug configuration context.
    /// </summary>
    public SlugConfigurationDataContext SlugConfigurationContext { get; }

    /// <summary>
    ///     The selected template.
    /// </summary>
    public Template? Template => this.TemplateInfo?.Template;

    /// <summary>
    ///     The template selection context.
    /// </summary>
    public TemplateSelectionDataContext TemplateSelectionContext { get; }

    /// <summary>
    ///     Whether the core configuration is enabled.
    /// </summary>
    public bool CoreConfigurationEnabled
    {
        get => this._coreConfigurationEnabled;
        set
        {
            this.RaiseAndSetIfChanged(ref this._coreConfigurationEnabled, value);
            if (value)
            {
                this.CoreConfigurationContext.EnableContext();
            }
            else
            {
                this.CoreConfigurationContext.ResetContext();
            }

            this.SlugConfigurationEnabled = false;
        }
    }

    /// <summary>
    ///     Whether the slug configuration is enabled.
    /// </summary>
    public bool SlugConfigurationEnabled
    {
        get => this._slugConfigurationEnabled;
        set
        {
            this.RaiseAndSetIfChanged(ref this._slugConfigurationEnabled, value);

            if (value)
            {
                this.SlugConfigurationContext.EnableContext();
            }
            else
            {
                this.SlugConfigurationContext.ResetContext();
            }
        }
    }

    /// <summary>
    ///     Clears the core log.
    /// </summary>
    public void ClearCoreLog()
    {
        if (this._coreLog == null)
        {
            return;
        }

        this._coreLog.Text = string.Empty;
    }

    /// <summary>
    ///     Clears the instruction log.
    /// </summary>
    public void ClearInstructionLog()
    {
        if (this._instructionsLog == null)
        {
            return;
        }

        this._instructionsLog.Text = string.Empty;
    }

    /// <summary>
    ///     Clears the script log.
    /// </summary>
    public void ClearScriptLog()
    {
        if (this._scriptLog == null)
        {
            return;
        }

        this._scriptLog.Text = string.Empty;
    }

    /// <summary>
    ///     Logs a message to the core log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>True if success, False otherwise.</returns>
    public bool LogToCore(string message)
    {
        if (this._coreLog == null)
        {
            return false;
        }

        this._coreLog.Text = $"{message}{Environment.NewLine}{this._coreLog.Text}";
        return true;
    }

    /// <summary>
    ///     Logs a message to the instruction log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>True if success, False otherwise.</returns>
    public bool LogToInstruction(string message)
    {
        if (this._instructionsLog == null)
        {
            return false;
        }

        this._instructionsLog.Text = $"{message}{Environment.NewLine}{this._instructionsLog.Text}";
        return true;
    }

    /// <summary>
    ///     Logs a message to the script log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>True if success, False otherwise.</returns>
    public bool LogToScript(string message)
    {
        if (this._scriptLog == null)
        {
            return false;
        }

        this._scriptLog.Text = $"{message}{Environment.NewLine}{this._scriptLog.Text}";
        return true;
    }
}
