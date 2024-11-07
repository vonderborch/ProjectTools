using System.Text.Json.Serialization;

namespace ProjectTools.Core.Templates;

/// <summary>
///     An abstract base template object.
/// </summary>
public abstract class AbstractTemplate
{
    /// <summary>
    ///     The author of the template.
    /// </summary>
    public string Author = string.Empty;

    /// <summary>
    ///     The description of the template.
    /// </summary>
    public string Description = string.Empty;

    /// <summary>
    ///     The name of the template.
    /// </summary>
    public string Name = string.Empty;

    /// <summary>
    ///     Paths to delete once a project is created or extended using this template.
    /// </summary>
    public List<string> PathsToRemove = [];

    /// <summary>
    ///     Paths that are excluded from the template.
    /// </summary>
    public List<string> PrepareExcludedPaths = [];

    /// <summary>
    ///     Paths to Python scripts to execute.
    /// </summary>
    public List<string> PythonScriptPaths = [];

    /// <summary>
    ///     Paths within the template that should only be renamed as needed, not having their contents updated.
    /// </summary>
    public List<string> RenameOnlyPaths = [];

    /// <summary>
    ///     The cached safe name of the template.
    /// </summary>
    [JsonIgnore] private string? safeName;

    /// <summary>
    ///     The name of the template builder to use.
    /// </summary>
    public string TemplateBuilder = string.Empty;

    /// <summary>
    ///     The current version of the template.
    /// </summary>
    public string Version = string.Empty;

    /// <summary>
    ///     Gets a file-system safe name for the template.
    /// </summary>
    [JsonIgnore]
    public string SafeName
    {
        get
        {
            if (this.safeName != null)
            {
                return this.safeName;
            }

            this.safeName = this.Name;
            this.safeName = this.safeName.Replace(" ", "_");
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                this.safeName = this.safeName.Replace(c, '-');
            }

            return this.safeName;
        }
    }
}
