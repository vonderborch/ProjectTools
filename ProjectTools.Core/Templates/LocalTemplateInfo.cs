using System.Text.Json.Serialization;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.TemplateRepositories;

namespace ProjectTools.Core.Templates;

public struct LocalTemplateInfo
{
    /// <summary>
    ///     The local path to the template.
    /// </summary>
    public string LocalPath;

    /// <summary>
    ///     The name of the template.
    /// </summary>
    public string Name;

    /// <summary>
    ///     The repo the template is from.
    /// </summary>
    public string Repo = string.Empty;

    /// <summary>
    ///     The SHA checksum of the template.
    /// </summary>
    public string Sha = string.Empty;

    /// <summary>
    ///     The size of the template in bytes.
    /// </summary>
    public ulong Size = 0;

    /// <summary>
    ///     The template object.
    /// </summary>
    [JsonIgnore] public Template Template = null;

    /// <summary>
    ///     The URL to the template.
    /// </summary>
    public string Url = string.Empty;

    /// <summary>
    ///     Returns True if the template is local-only, False otherwise.
    /// </summary>
    [JsonIgnore]
    public bool IsLocalOnlyTemplate => string.IsNullOrEmpty(this.Url);

    /// <summary>
    ///     Creates a new instance of the <see cref="LocalTemplateInfo" /> struct.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="localPath">The local path.</param>
    /// <param name="size">The size, in bytes.</param>
    /// <param name="template">The template.</param>
    public LocalTemplateInfo(string name, string localPath, ulong size, Template template)
    {
        this.LocalPath = localPath;
        this.Name = name;
        this.Size = size;
        this.Template = template;
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="LocalTemplateInfo" /> struct.
    /// </summary>
    /// <param name="templateMetadata">The git template metadata.</param>
    /// <param name="localPath">The local path.</param>
    /// <param name="template">The template.</param>
    public LocalTemplateInfo(GitTemplateMetadata templateMetadata, string localPath, Template template)
    {
        this.Name = templateMetadata.Name;
        this.Url = templateMetadata.Url;
        this.Size = (ulong)templateMetadata.Size;
        this.Repo = templateMetadata.Repo;
        this.Sha = templateMetadata.Sha;
        this.LocalPath = localPath;
        this.Template = template;
    }

    /// <summary>
    ///     Loads the template object from the template file.
    /// </summary>
    /// <returns>The template object.</returns>
    /// <exception cref="Exception">Raises if no template was found in the file.</exception>
    public Template LoadTemplate()
    {
        var template =
            JsonHelpers.DeserializeFromArchivedFile<Template>(this.LocalPath,
                TemplateConstants.TemplateSettingsFileName);

        this.Template = template ?? throw new Exception("Invalid template file!");
        return this.Template;
    }
}
