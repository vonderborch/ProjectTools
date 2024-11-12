using System.Diagnostics;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core.TemplateRepositories;

/// <summary>
///     Represents the result of a template update check.
/// </summary>
[DebuggerDisplay(
    "New: {NewTemplates.Count} | Updateable: {UpdateableTemplates.Count} | Orphaned: {OrphanedTemplates.Count}")]
public struct TemplateUpdateCheckResult
{
    /// <summary>
    ///     The new templates.
    /// </summary>
    public List<GitTemplateMetadata> NewTemplates = new();

    /// <summary>
    ///     The orphaned templates.
    /// </summary>
    public List<LocalTemplateInfo> OrphanedTemplates = new();

    /// <summary>
    ///     The total number of remote templates processed.
    /// </summary>
    public int TotalRemoteTemplatesProcessed = -1;

    /// <summary>
    ///     The updateable templates.
    /// </summary>
    public List<GitTemplateMetadata> UpdateableTemplates = new();

    /// <summary>
    ///     Creates a new instance of the <see cref="TemplateUpdateCheckResult" /> class.
    /// </summary>
    public TemplateUpdateCheckResult()
    {
    }

    /// <summary>
    ///     The number of templates needing download.
    /// </summary>
    public int TotalTemplatesNeedingDownload => this.NewTemplates.Count + this.UpdateableTemplates.Count;

    /// <summary>
    ///     Gets the total size of all new templates.
    /// </summary>
    public ulong NewTemplateSize
    {
        get
        {
            var totalSize = 0UL;
            foreach (var template in this.NewTemplates)
            {
                totalSize += (ulong)template.Size;
            }

            return totalSize;
        }
    }

    /// <summary>
    ///     The total size of all new templates in megabytes.
    /// </summary>
    public double NewTemplateSizeInMegabytes => ToMegabytes(this.NewTemplateSize);

    /// <summary>
    ///     The total size of all new and updateable templates.
    /// </summary>
    public ulong TotalSize
    {
        get
        {
            var totalSize = this.NewTemplateSize + this.UpdateableTemplateSize;

            return totalSize;
        }
    }

    /// <summary>
    ///     Gets the total size of all updateable templates.
    /// </summary>
    public ulong UpdateableTemplateSize
    {
        get
        {
            var totalSize = 0UL;
            foreach (var template in this.UpdateableTemplates)
            {
                totalSize += (ulong)template.Size;
            }

            return totalSize;
        }
    }

    /// <summary>
    ///     The total size of all updateable templates in megabytes.
    /// </summary>
    public double UpdateableTemplateSizeInMegabytes => ToMegabytes(this.UpdateableTemplateSize);

    /// <summary>
    ///     Converts a size in bytes to megabytes.
    /// </summary>
    /// <param name="size">The size.</param>
    /// <returns>The size.</returns>
    private double ToMegabytes(ulong size)
    {
        return size / 1024d / 1024d;
    }
}
