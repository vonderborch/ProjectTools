using System.Text.Json.Serialization;
using Octokit;
using ProjectTools.Core.Helpers;

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
    public string Sha;

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
    /// <param name="info">The git content information.</param>
    /// <param name="repo">The repo the template is located in.</param>
    public GitTemplateMetadata(RepositoryContent info, string repo)
    {
        this.Name = Path.GetFileNameWithoutExtension(info.Name);
        this.Sha = info.Sha;
        this.Url = info.DownloadUrl;
        this.Repo = repo;
        this.Size = info.Size;
    }

    /// <summary>
    ///     Gets the display name.
    /// </summary>
    [JsonIgnore]
    public string DisplayName => Path.GetFileNameWithoutExtension(this.Name);

    /// <summary>
    ///     Gets the safe name.
    /// </summary>
    [JsonIgnore]
    public string SafeName => IOHelpers.GetFileSystemSafeString(this.Name);

    /// <summary>
    ///     Gets the size in MB.
    /// </summary>
    [JsonIgnore]
    public double SizeInMb => this.Size / 1024d / 1024d;
}
