using Octokit;
using System.Diagnostics;

namespace ProjectTools.Core.Internal.Repositories
{
    /// <summary>
    /// Information on the template's git copy
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class TemplateGitInfo
    {
        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name;

        /// <summary>
        /// The SHA checksum for the template
        /// </summary>
        public string SHA;

        /// <summary>
        /// The URL for the template
        /// </summary>
        public string Url;

        /// <summary>
        /// The repo the template is located in
        /// </summary>
        public string Repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGitInfo"/> class.
        /// </summary>
        public TemplateGitInfo()
        {
            Name = string.Empty;
            SHA = string.Empty;
            Url = string.Empty;
            Repo = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateGitInfo"/> class.
        /// </summary>
        /// <param name="info">The git content information.</param>
        /// <param name="repo">The repo the template is located in.</param>
        public TemplateGitInfo(RepositoryContent info, string repo)
        {
            Name = info.Name;
            SHA = info.Sha;
            Url = info.DownloadUrl;
            Repo = repo;
        }
    }
}
