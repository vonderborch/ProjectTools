using System.Diagnostics;

namespace ProjectTools.Core.Internal.Repositories
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
        private List<TemplateGitInfo> _templateCache = new();

        /// <summary>
        /// The most recent template map cache for for the monitored repositories
        /// </summary>
        private Dictionary<string, TemplateGitInfo> _templateMapCache = new();

        public List<string> RepositoryNames { get; }

        public List<Repository> Repositories { get; }

        public RepositoryCollection(List<string> repositories, bool autoLoad = true)
        {
            RepositoryNames = repositories;
            Repositories = (from repo in repositories select new Repository(repo, autoLoad)).ToList();
        }

        public List<TemplateGitInfo> TemplatesList
        {
            get
            {
                if (_templateCache.Count == 0)
                {
                    _templateCache = GetTemplateInfoForRepository(true);
                }

                return _templateCache;
            }
        }

        public Dictionary<string, TemplateGitInfo> TemplateMap
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

        public List<TemplateGitInfo> GetTemplateInfoForRepository(bool forceLoad = true)
        {
            var output = new List<TemplateGitInfo>();
            foreach (var repo in Repositories)
            {
                output.AddRange(repo.GetTemplateInfoForRepository(forceLoad));
            }

            _templateCache = output;
            _templateMapCache = new();
            return output;
        }
    }
}
