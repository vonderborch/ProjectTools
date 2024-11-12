using System.Diagnostics;

namespace ProjectTools.Core.Templates;

/// <summary>
///     A template, used to create or extend projects.
/// </summary>
[DebuggerDisplay("{Name}")]
public class Template : AbstractTemplate
{
    /// <summary>
    ///     Information on slugs for this template, used to replace instances of the slug with the value of the slug.
    /// </summary>
    public List<Slug> Slugs = [];
}
