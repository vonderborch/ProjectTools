#region

using System.Collections.Generic;
using ReactiveUI;

#endregion

namespace ProjectTools.App.DataContexts.GenerateProjectSubContexts;

/// <summary>
///     The slug configuration data context.
/// </summary>
public class SlugConfigurationDataContext : ReactiveObject
{
    /// <summary>
    /// The slugs to edit.
    /// </summary>
    private List<string> _slugsToEdit = new();
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="SlugConfigurationDataContext" /> class.
    /// </summary>
    /// <param name="parentContext">The parent context.</param>
    public SlugConfigurationDataContext(GenerateProjectDataContext parentContext)
    {
        this.ParentContext = parentContext;
    }

    /// <summary>
    ///     The parent context.
    /// </summary>
    public GenerateProjectDataContext ParentContext { get; }

    /// <summary>
    /// The slugs to edit.
    /// </summary>
    public List<string> SlugsToEdit
    {
        get => this._slugsToEdit;
        set => this.RaiseAndSetIfChanged(ref this._slugsToEdit, value);
    }

    public void EnableContext()
    {
        ResetContext();
    }

    public void ResetContext()
    {
        SlugsToEdit = new();
    }
}
