#region

using System;
using System.Linq;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.PrepareTemplateSubContexts;

/// <summary>
///     The data context for the Template Configuration control.
/// </summary>
public class TemplateConfigurationDataContext : ReactiveObject
{
    /// <summary>
    ///     The parent context.
    /// </summary>
    private readonly PrepareTemplateDataContext _parentContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateConfigurationDataContext" /> class.
    /// </summary>
    /// <param name="parentContext">The parent context.</param>
    public TemplateConfigurationDataContext(PrepareTemplateDataContext parentContext)
    {
        this._parentContext = parentContext;
    }

    /// <summary>
    ///     The template author.
    /// </summary>
    public string Author
    {
        get => this._parentContext.PreparationTemplate?.Author ?? string.Empty;
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.Author = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template description.
    /// </summary>
    public string Description
    {
        get => this._parentContext.PreparationTemplate?.Description ?? string.Empty;
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.Description = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template instructions.
    /// </summary>
    public string Instructions
    {
        get
        {
            if (this._parentContext.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.Instructions);
        }
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.Instructions = value.Split(Environment.NewLine).ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template name.
    /// </summary>
    public string Name
    {
        get => this._parentContext.PreparationTemplate?.Name ?? string.Empty;
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.Name = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template paths to remove.
    /// </summary>
    public string PathsToRemove
    {
        get
        {
            if (this._parentContext.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.PathsToRemove);
        }
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.PathsToRemove = value.Split(Environment.NewLine).ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template prepare excluded paths.
    /// </summary>
    public string PrepareExcludedPaths
    {
        get
        {
            if (this._parentContext.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.PrepareExcludedPaths);
        }
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.PrepareExcludedPaths =
                    value.Split(Environment.NewLine).ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template python script paths.
    /// </summary>
    public string PythonScriptPaths
    {
        get
        {
            if (this._parentContext.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.PythonScriptPaths);
        }
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.PythonScriptPaths = value.Split(Environment.NewLine).ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template rename only paths.
    /// </summary>
    public string RenameOnlyPaths
    {
        get
        {
            if (this._parentContext.PreparationTemplate is null)
            {
                return string.Empty;
            }

            return string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.RenameOnlyPaths);
        }
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.RenameOnlyPaths = value.Split(Environment.NewLine).ToList();
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     The template version.
    /// </summary>
    public string Version
    {
        get => this._parentContext.PreparationTemplate?.Version ?? string.Empty;
        set
        {
            if (this._parentContext.PreparationTemplate is not null)
            {
                this._parentContext.PreparationTemplate.Version = value;
            }

            this.RaisePropertyChanged();
        }
    }

    /// <summary>
    ///     Clears the data context.
    /// </summary>
    public void ClearContext()
    {
        this.Name = string.Empty;
        this.Version = string.Empty;
        this.Author = string.Empty;
        this.Description = string.Empty;
        this.RenameOnlyPaths = string.Empty;
        this.PathsToRemove = string.Empty;
        this.PrepareExcludedPaths = string.Empty;
        this.PythonScriptPaths = string.Empty;
        this.Instructions = string.Empty;
    }

    /// <summary>
    ///     Enables the data context.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void EnableContext()
    {
        if (this._parentContext.PreparationTemplate is null)
        {
            throw new Exception("PreparationTemplate is null!");
        }

        this.Name = this._parentContext.PreparationTemplate.Name;
        this.Version = this._parentContext.PreparationTemplate.Version;
        this.Description = this._parentContext.PreparationTemplate.Description;
        this.Author = this._parentContext.PreparationTemplate.Author;
        this.RenameOnlyPaths =
            string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.RenameOnlyPaths);
        this.PathsToRemove = string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.PathsToRemove);
        this.PrepareExcludedPaths =
            string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.PrepareExcludedPaths);
        this.PythonScriptPaths =
            string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.PythonScriptPaths);
        this.Instructions = string.Join(Environment.NewLine, this._parentContext.PreparationTemplate.Instructions);
    }
}
