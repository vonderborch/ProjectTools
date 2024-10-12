using System.Diagnostics;

namespace ProjectTools.CoreOLD.Templating.Repositories
{
    /// <summary>
    /// A collection of git template repositories
    /// </summary>
    [DebuggerDisplay("{Repositories.Count}")]
    public class RepositoryCollection
    {
        /// <summary>
        /// The most recent list of templates for the monitored repositories
        /// </summary>
        private List<TemplateGitMetadata> _templateCache = [];

        /// <summary>
        /// The most recent template map cache for for the monitored repositories
        /// </summary>
        private Dictionary<string, TemplateGitMetadata> _templateMapCache = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryCollection"/> class.
        /// </summary>
        /// <param name="repositories">The repositories.</param>
        /// <param name="autoLoad">if set to <c>true</c> [automatic load].</param>
        public RepositoryCollection(List<string> repositories, bool autoLoad = true)
        {
            RepositoryNames = repositories;
            Repositories = (from repo in repositories select new Repository(repo, autoLoad)).ToList();
        }

        /// <summary>
        /// Gets the repositories.
        /// </summary>
        /// <value>The repositories.</value>
        public List<Repository> Repositories { get; }

        /// <summary>
        /// Gets the repository names.
        /// </summary>
        /// <value>The repository names.</value>
        public List<string> RepositoryNames { get; }

        /// <summary>
        /// Gets the template map.
        /// </summary>
        /// <value>The template map.</value>
        public Dictionary<string, TemplateGitMetadata> TemplateMap
        {
            get
            {
                if (_templateMapCache.Count == 0)
                {
                    _templateMapCache = _templateCache.ToDictionary(x => x.Name, x => x);
                }

                return _templateMapCache;
            }
        }

        /// <summary>
        /// Gets the templates list.
        /// </summary>
        /// <value>The templates list.</value>
        public List<TemplateGitMetadata> TemplatesList
        {
            get
            {
                if (_templateCache.Count == 0)
                {
                    _templateCache = GetTemplateInfoForRepositories(true);
                }

                return _templateCache;
            }
        }

        /// <summary>
        /// Gets the template information for repository.
        /// </summary>
        /// <param name="forceLoad">if set to <c>true</c> [force load].</param>
        /// <returns>Template info for the repository</returns>
        public List<TemplateGitMetadata> GetTemplateInfoForRepositories(bool forceLoad = true)
        {
            var output = new List<TemplateGitMetadata>();
            foreach (var repo in Repositories)
            {
                output.AddRange(repo.GetTemplateInfoForRepository(forceLoad));
            }

            _templateCache = output;
            _templateMapCache = [];
            return output;
        }
    }
}
