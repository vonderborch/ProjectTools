using Octokit;
using System.Diagnostics;

namespace ProjectTools.Core.Templating.Repositories
{
    /// <summary>
    /// Metadata about a template in a git repository
    /// </summary>
    [DebuggerDisplay("{Name} | {Repo}")]
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
        }
    }
}
