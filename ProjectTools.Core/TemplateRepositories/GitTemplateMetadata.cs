using System.Text.Json.Serialization;
using Octokit;

namespace ProjectTools.Core.TemplateRepositories;

/// <summary>
///     Metadata about a template in a git repository
/// </summary>
public class GitTemplateMetadata
{
    /// <summary>
    ///     The name of the template
    /// </summary>
    public string Name;

    /// <summary>
    ///     The repo the template is located in
    /// </summary>
    public string Repo;

    /// <summary>
    ///     The SHA checksum for the template
    /// </summary>
    public string SHA;

    /// <summary>
    ///     The size
    /// </summary>
    public int Size;

    /// <summary>
    ///     The URL for the template
    /// </summary>
    public string Url;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GitTemplateMetadata" /> class.
    /// </summary>
    public GitTemplateMetadata()
    {
        this.Name = string.Empty;
        this.SHA = string.Empty;
        this.Url = string.Empty;
        this.Repo = string.Empty;
        this.Size = 0;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="GitTemplateMetadata" /> class.
    /// </summary>
    /// <param name="info">The git content information.</param>
    /// <param name="repo">The repo the template is located in.</param>
    public GitTemplateMetadata(RepositoryContent info, string repo)
    {
        this.Name = info.Name;
        this.SHA = info.Sha;
        this.Url = info.DownloadUrl;
        this.Repo = repo;
        this.Size = info.Size;
    }

    /// <summary>
    ///     Gets the display name.
    /// </summary>
    /// <value>The display name.</value>
    [JsonIgnore]
    public string DisplayName => Path.GetFileNameWithoutExtension(this.Name);
}
