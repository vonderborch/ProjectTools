using System.Diagnostics;
using Octokit;
using ProjectTools.Core.Constants;

namespace ProjectTools.Core.TemplateRepositories;

/// <summary>
///     Represents the contents of a git repo
/// </summary>
[DebuggerDisplay("{Info}")]
public class GitRepoContents
{
    /// <summary>
    ///     The child repository content
    /// </summary>
    public List<GitRepoContents> ChildContent;

    /// <summary>
    ///     The repository content at this level
    /// </summary>
    public RepositoryContent Info;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GitRepoContents" /> class.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="owner">The owner.</param>
    /// <param name="name">The name.</param>
    /// <param name="path">The path.</param>
    /// <param name="currentDepth">The current depth.</param>
    public GitRepoContents(RepositoryContent info, string owner, string name, string path, int currentDepth = 0)
    {
        this.Info = info;

        if (info.Type == ContentType.Dir && currentDepth < TemplateConstants.MaxGitRepoTemplateSearchDepth)
        {
            var childContents = Manager.Instance.GitClient.Repository.Content.GetAllContents(owner, name, path).Result;

            this.ChildContent = childContents.Select(c => new GitRepoContents(c, owner, name, c.Path, currentDepth + 1))
                .ToList();
        }
        else
        {
            this.ChildContent = [];
        }
    }

    /// <summary>
    ///     Converts the contents to a string.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return this.Info.ToString() ?? string.Empty;
    }
}
