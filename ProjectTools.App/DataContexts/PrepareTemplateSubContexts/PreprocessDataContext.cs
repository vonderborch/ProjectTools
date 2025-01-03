#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.PrepareTemplateSubContexts;

/// <summary>
///     The data context for the Preprocess control.
/// </summary>
public class PreprocessDataContext : ReactiveObject
{
    /// <summary>
    ///     The parent context.
    /// </summary>
    private readonly PrepareTemplateDataContext _parentContext;

    /// <summary>
    ///     The available template builders.
    /// </summary>
    private List<string> _availableTemplateBuilders;

    /// <summary>
    ///     The template directory.
    /// </summary>
    private string _directory = string.Empty;

    /// <summary>
    ///     Whether to force overwrite.
    /// </summary>
    private bool _forceOverwrite;

    /// <summary>
    ///     The output directory.
    /// </summary>
    private string _outputDirectory = string.Empty;

    /// <summary>
    ///     The selected template builder index.
    /// </summary>
    private int _selectedTemplateBuilderIndex;

    /// <summary>
    ///     Whether to skip cleaning.
    /// </summary>
    private bool _skipCleaning;

    /// <summary>
    ///     Whether to run in WhatIf mode.
    /// </summary>
    private bool _whatIf;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PreprocessDataContext" /> class.
    /// </summary>
    /// <param name="parent">The parent context.</param>
    public PreprocessDataContext(PrepareTemplateDataContext parent)
    {
        this._parentContext = parent;

        var availableTemplateBuilders = parent.TemplatePreparer.GetTemplateBuilders();
        var options = availableTemplateBuilders.Select(x => x.Name).ToList();
        options.Insert(0, "auto");
        this._availableTemplateBuilders = options;
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
    ///     The Template Directory.
    /// </summary>
    public string Directory
    {
        get => this._directory;
        set
        {
            if (string.IsNullOrEmpty(this._outputDirectory) ||
                Path.GetDirectoryName(this._directory) == this.OutputDirectory)
            {
                this.OutputDirectory = Path.GetDirectoryName(value);
            }

            this.RaiseAndSetIfChanged(ref this._directory, value);
        }
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
    ///     The Output Directory.
    /// </summary>
    public string OutputDirectory
    {
        get => this._outputDirectory;
        set => this.RaiseAndSetIfChanged(ref this._outputDirectory, value);
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
    ///     Whether to skip cleaning.
    /// </summary>
    public bool SkipCleaning
    {
        get => this._skipCleaning;
        set => this.RaiseAndSetIfChanged(ref this._skipCleaning, value);
    }

    /// <summary>
    ///     Whether to run in WhatIf mode.
    /// </summary>
    public bool WhatIf
    {
        get => this._whatIf;
        set => this.RaiseAndSetIfChanged(ref this._whatIf, value);
    }
}
