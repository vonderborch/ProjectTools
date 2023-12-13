using System.Diagnostics;
using System.Text.Json.Serialization;
using Octokit;

namespace ProjectTools.Core.Templating.Repositories
{
    /// <summary>
    /// Metadata about a template in a git repository
    /// </summary>
    [DebuggerDisplay("{DisplayName} | {Repo}")]
    public class TemplateGitMetadata
    {
        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name;

        /// <summary>
        /// The repo the template is located in
        /// </summary>
        public string Repo;

        /// <summary>
        /// The SHA checksum for the template
        /// </summary>
        public string SHA;

        /// <summary>
        /// The size
        /// </summary>
        public int Size;

        /// <summary>
        /// The URL for the template
        /// </summary>
        public string Url;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGitMetadata"/> class.
        /// </summary>
        public TemplateGitMetadata()
        {
            Name = string.Empty;
            SHA = string.Empty;
            Url = string.Empty;
            Repo = string.Empty;
            Size = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGitMetadata"/> class.
        /// </summary>
        /// <param name="info">The git content information.</param>
        /// <param name="repo">The repo the template is located in.</param>
        public TemplateGitMetadata(RepositoryContent info, string repo)
        {
            Name = info.Name;
            SHA = info.Sha;
            Url = info.DownloadUrl;
            Repo = repo;
            Size = info.Size;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [JsonIgnore]
        public string DisplayName => Path.GetFileNameWithoutExtension(Name);
    }
}
