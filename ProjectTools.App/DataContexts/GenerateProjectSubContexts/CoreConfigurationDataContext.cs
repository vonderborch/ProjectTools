#region

using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.GenerateProjectSubContexts;

/// <summary>
///     The core configuration data context.
/// </summary>
public class CoreConfigurationDataContext : ReactiveObject
{
    /// <summary>
    ///     Whether to force overwrite.
    /// </summary>
    private bool _forceOverwrite;

    /// <summary>
    ///     The parent output directory.
    /// </summary>
    private string _parentOutputDirectory;

    /// <summary>
    ///     The project name.
    /// </summary>
    private string _projectName;

    /// <summary>
    ///     Whether to run in what-if mode.
    /// </summary>
    private bool _whatIf;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CoreConfigurationDataContext" /> class.
    /// </summary>
    /// <param name="parentContext">The parent context.</param>
    public CoreConfigurationDataContext(GenerateProjectDataContext parentContext)
    {
        this.ParentContext = parentContext;

        this._projectName = string.Empty;
        this._parentOutputDirectory = string.Empty;
    }

    /// <summary>
    ///     The parent context.
    /// </summary>
    public GenerateProjectDataContext ParentContext { get; }

    /// <summary>
    ///     Whether to force overwrite.
    /// </summary>
    public bool ForceOverwrite
    {
        get => this._forceOverwrite;
        set => this.RaiseAndSetIfChanged(ref this._forceOverwrite, value);
    }

    /// <summary>
    ///     The parent output directory.
    /// </summary>
    public string ParentOutputDirectory
    {
        get => this._parentOutputDirectory;
        set => this.RaiseAndSetIfChanged(ref this._parentOutputDirectory, value);
    }

    /// <summary>
    ///     The project name.
    /// </summary>
    public string ProjectName
    {
        get => this._projectName;
        set => this.RaiseAndSetIfChanged(ref this._projectName, value);
    }

    /// <summary>
    ///     Whether to run in what-if mode.
    /// </summary>
    public bool WhatIf
    {
        get => this._whatIf;
        set => this.RaiseAndSetIfChanged(ref this._whatIf, value);
    }

    public void EnableContext()
    {
        ResetContext();
    }

    public void ResetContext()
    {
        this.ProjectName = string.Empty;
        this.ParentOutputDirectory = string.Empty;
        this.ForceOverwrite = false;
        this.WhatIf = false;
    }
}
