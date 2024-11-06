namespace ProjectTools.Core.Constants;

/// <summary>
/// Constants related to directories.
/// </summary>
public static class PathConstants
{
    /// <summary>
    /// The core directory for the program
    /// </summary>
    public static readonly string CoreDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProjectTools");

    /// <summary>
    ///     The file extension for templates.
    /// </summary>
    public static readonly string TemplateFileExtension = "ptt";
}
