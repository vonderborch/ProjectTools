using ProjectTools.Core.Implementations;
using ProjectTools.Core.Templating.Preparation;
using ProjectTools.Core.Templating.Repositories;

namespace ProjectTools.Core.Templating
{
    /// <summary>
    /// Class to handle templating logic (Generation and Preperation)
    /// </summary>
    public class Templater
    {
        /// <summary>
        /// The template unique identifier counts
        /// </summary>
        public Dictionary<string, int> TemplateGuidCounts = new();

        /// <summary>
        /// The template repositories collection
        /// </summary>
        private RepositoryCollection? _repositories;

        /// <summary>
        /// The settings
        /// </summary>
        private Settings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Templater"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public Templater(Settings settings)
        {
            _settings = settings;
            _repositories = null;
            TemplateGuidCounts = new();
        }

        /// <summary>
        /// Gets the template preparer.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns></returns>
        public AbstractTemplatePreparer GetTemplatePreparer(Implementation implementation)
        {
            return TemplatePreperationFactory.GetTemplatePreparer(implementation);
        }
    }
}
