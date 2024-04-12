using System.Diagnostics;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Templating.Repositories;

/// <summary>
/// A template with additional information on local and remote data
/// </summary>
[DebuggerDisplay("{Template} | {FilePath}")]
public class MetaTemplate
{
    /// <summary>
    /// The local file path
    /// </summary>
    public required string FilePath = string.Empty;

    /// <summary>
    /// The implementation for this template
    /// </summary>
    public required Implementation Implementation;

    /// <summary>
    /// The repo information
    /// </summary>
    public TemplateGitMetadata? RepoInfo = null;

    /// <summary>
    /// The template
    /// </summary>
    public required Template Template;
}