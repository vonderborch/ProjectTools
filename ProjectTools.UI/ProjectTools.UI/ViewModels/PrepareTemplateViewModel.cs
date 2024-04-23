namespace ProjectTools.ViewModels;

/// <summary>
/// The view model for the PrepareTemplateView.
/// </summary>
public class PrepareTemplateViewModel : ViewModelBase
{
    /// <summary>
    /// The view model property that represents the directory of the project to be templated.
    /// </summary>
    public string Directory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the output directory.
    /// </summary>
    /// <value>
    /// The output directory.
    /// </value>
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to skip the cleaning process.
    /// </summary>
    /// <remarks>
    /// The cleaning process involves removing any unnecessary files or directories.
    /// By default, the cleaning process is not skipped (set to false).
    /// </remarks>
    public bool SkipCleaning { get; set; } = false;

    /// <summary>
    /// Gets or sets a boolean value that indicates whether the "What If" mode is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if the "What If" mode is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool WhatIf { get; set; } = false;

    public string Output { get; set; } = string.Empty;
}