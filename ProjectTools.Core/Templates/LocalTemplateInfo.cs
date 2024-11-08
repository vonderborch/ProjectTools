using System.Text.Json.Serialization;

namespace ProjectTools.Core.Templates;

public class LocalTemplateInfo
{
    /// <summary>
    ///     The local path to the template.
    /// </summary>
    public required string LocalPath;

    /// <summary>
    ///     The name of the template.
    /// </summary>
    public required string Name;

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
}
