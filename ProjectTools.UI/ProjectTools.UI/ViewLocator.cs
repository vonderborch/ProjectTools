using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ProjectTools.ViewModels;

namespace ProjectTools;

/// <summary>
/// Represents a class used to locate and build views based on view models.
/// </summary>
public class ViewLocator : IDataTemplate
{
    /// <summary>
    /// Builds a view based on the given view model.
    /// </summary>
    /// <param name="data">The view model object.</param>
    /// <returns>The built view.</returns>
    public Control Build(object data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null) return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = name };
    }

    /// <summary>
    /// Determines if the given object matches the criteria for a view model.
    /// </summary>
    /// <param name="data">The object to check.</param>
    /// <returns>True if the object is a view model; otherwise, false.</returns>
    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}