#region

using ProjectTools.Core.Constants;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.GenerateProjectSubContexts;

/// <summary>
///     The core configuration data context.
/// </summary>
public class CoreConfigurationDataContext : ReactiveObject
{
    /// <summary>
    ///     Whether advanced options are enabled.
    /// </summary>
    private bool _advancedOptionsEnabled;

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
    ///     Whether to remove the existing project generation configuration file.
    /// </summary>
    private bool _removeExistingProjectGenerationConfigurationFile = true;

    /// <summary>
    ///     The specific project generation configuration file.
    /// </summary>
    private string _specificProjectGenerationConfigFile;

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
        this._specificProjectGenerationConfigFile = PathConstants.DefaultProjectGenerationConfigFileName;
    }

    /// <summary>
    ///     The parent context.
    /// </summary>
    public GenerateProjectDataContext ParentContext { get; }

    /// <summary>
    ///     Whether advanced options are enabled.
    /// </summary>
    public bool AdvancedOptionsEnabled
    {
        get => this._advancedOptionsEnabled;
        set => this.RaiseAndSetIfChanged(ref this._advancedOptionsEnabled, value);
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
    ///     Whether to remove the existing project generation configuration file.
    /// </summary>
    public bool RemoveExistingProjectGenerationConfigurationFile
    {
        get => this._removeExistingProjectGenerationConfigurationFile;
        set => this.RaiseAndSetIfChanged(ref this._removeExistingProjectGenerationConfigurationFile, value);
    }

    /// <summary>
    ///     The specific project generation configuration file.
    /// </summary>
    public string SpecificProjectGenerationConfigFile
    {
        get => this._specificProjectGenerationConfigFile;
        set => this.RaiseAndSetIfChanged(ref this._specificProjectGenerationConfigFile, value);
    }

    /// <summary>
    ///     Whether to run in what-if mode.
    /// </summary>
    public bool WhatIf
    {
        get => this._whatIf;
        set => this.RaiseAndSetIfChanged(ref this._whatIf, value);
    }

    /// <summary>
    ///     Enables the context.
    /// </summary>
    public void EnableContext()
    {
        ResetContext();
    }

    /// <summary>
    ///     Resets the context.
    /// </summary>
    public void ResetContext()
    {
        this.ProjectName = string.Empty;
        this.ParentOutputDirectory = string.Empty;
        this.ForceOverwrite = false;
        this.WhatIf = false;
        this.AdvancedOptionsEnabled = false;
        this.SpecificProjectGenerationConfigFile = PathConstants.DefaultProjectGenerationConfigFileName;
        this.RemoveExistingProjectGenerationConfigurationFile = true;
    }
}
