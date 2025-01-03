#region

using System;
using Avalonia.Controls;
using ProjectTools.App.DataContexts.GenerateProjectSubContexts;
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

    public GenerateProjectDataContext(SelectableTextBlock? coreLog = null, SelectableTextBlock? scriptLog = null,
        SelectableTextBlock? instructionsLog = null)
    {
        this._coreLog = coreLog;
        this._scriptLog = scriptLog;
        this._instructionsLog = instructionsLog;
        this.TemplateSelectionContext = new TemplateSelectionDataContext();
    }

    public TemplateSelectionDataContext TemplateSelectionContext { get; }

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
