using System.Diagnostics;

namespace ProjectTools.CoreOLD.Templating.Repositories
{
    [DebuggerDisplay("New: {NewTemplates.Count} | Updateable: {UpdateableTemplates.Count} | Orphaned: {OrphanedTemplates.Count}")]
    public readonly struct TemplateUpdateResult
    {
        /// <summary>
        /// Creates new templates.
        /// </summary>
        public readonly List<TemplateGitMetadata> NewTemplates;

        /// <summary>
        /// The orphaned templates
        /// </summary>
        public readonly List<string> OrphanedTemplates;

        /// <summary>
        /// The total remote templates processed
        /// </summary>
        public readonly int TotalRemoteTemplatesProcessed;

        /// <summary>
        /// The updateable templates
        /// </summary>
        public readonly List<TemplateGitMetadata> UpdateableTemplates;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateUpdateResult"/> struct.
        /// </summary>
        /// <param name="totalRemoteTemplatesProcessed">The total remote templates processed.</param>
        /// <param name="newTemplates">The new templates.</param>
        /// <param name="updateableTemplates">The updateable templates.</param>
        /// <param name="orphanedTemplates">The orphaned templates.</param>
        public TemplateUpdateResult(int totalRemoteTemplatesProcessed, List<TemplateGitMetadata> newTemplates, List<TemplateGitMetadata> updateableTemplates, List<string> orphanedTemplates)
        {
            TotalRemoteTemplatesProcessed = totalRemoteTemplatesProcessed;
            NewTemplates = newTemplates;
            UpdateableTemplates = updateableTemplates;
            OrphanedTemplates = orphanedTemplates;
        }

        /// <summary>
        /// Gets the total size of all new templates.
        /// </summary>
        /// <value>The total size.</value>
        public ulong NewTemplateSize
        {
            get
            {
                var totalSize = 0UL;
                foreach (var template in NewTemplates)
                {
                    totalSize += (ulong)template.Size;
                }
                return totalSize;
            }
        }

        /// <summary>
        /// Gets the total size of all new and updateable templates.
        /// </summary>
        /// <value>The total size.</value>
        public ulong TotalSize
        {
            get
            {
                var totalSize = NewTemplateSize + UpdateableTemplateSize;

                return totalSize;
            }
        }

        /// <summary>
        /// Gets the total size of all updateable templates.
        /// </summary>
        /// <value>The total size.</value>
        public ulong UpdateableTemplateSize
        {
            get
            {
                var totalSize = 0UL;
                foreach (var template in UpdateableTemplates)
                {
                    totalSize += (ulong)template.Size;
                }
                return totalSize;
            }
        }
    }
}
