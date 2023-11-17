using Octokit;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ProjectTools.Core.Internal.Repositories
{
    /// <summary>
    /// Handles storing and downloading info on templates in a given repository.
    /// </summary>
    [DebuggerDisplay("{Repository} | {Templates.Count}")]
    public class Repository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="repository">The repository to check for templates.</param>
        /// <param name="autoLoad">True to check for templates on initialization, false otherwise. Defaults to true.</param>
        public Repository(string repository, bool autoLoad = true)
        {
            Repo = repository;
            var splitRepositoryName = repository.Split('/');
            RepositoryOwner = splitRepositoryName[splitRepositoryName.Length - 2];
            RepositoryName = splitRepositoryName[splitRepositoryName.Length - 1];

            Templates = new();

            if (autoLoad)
            {
                GetTemplateInfoForRepository(true);
            }
        }

        /// <summary>
        /// The repository stored/handled
        /// </summary>
        public string Repo { get; }

        /// <summary>
        /// The name of the repository
        /// </summary>
        public string RepositoryName { get; }

        /// <summary>
        /// The owner of the repository
        /// </summary>
        public string RepositoryOwner { get; }

        /// <summary>
        /// A dictionary representing the templates, keyed by template name
        /// </summary>
        /// <value>The template map.</value>
        [JsonIgnore]
        public Dictionary<string, TemplateGitInfo> TemplateMap =>
            Templates.ToDictionary(t => t.Name, t => t);

        /// <summary>
        /// The list of loaded templates
        /// </summary>
        public List<TemplateGitInfo> Templates { get; private set; }

        /// <summary>
        /// Gets information on templates stored in the repository this instance represents.
        /// </summary>
        /// <param name="forceLoad">
        /// True to scan the repository to templates, false to not scan the repository if we already have templates
        /// loaded. Defaults to true.
        /// </param>
        /// <returns>A list of templates discovered in the repository.</returns>
        public List<TemplateGitInfo> GetTemplateInfoForRepository(bool forceLoad = true)
        {
            if (forceLoad || Templates.Count == 0)
            {
                var rootContents = Manager.Instance.GitClient.Repository.Content
                    .GetAllContents(RepositoryOwner, RepositoryName)
                    .Result.ToList();

                var repoContents = rootContents
                    .Select(x => new GitRepoContents(x, RepositoryOwner, RepositoryName, x.Path))
                    .ToList();

                Templates = GetTemplateInfoForContents(repoContents);
            }

            return Templates;
        }

        /// <summary>
        /// Checks git contents for templates.
        /// </summary>
        /// <param name="contents">The contents to check for templates</param>
        /// <returns>A list of templates the git contents store</returns>
        private List<TemplateGitInfo> GetTemplateInfoForContents(List<GitRepoContents> contents)
        {
            var output = new List<TemplateGitInfo>();

            foreach (var content in contents)
            {
                if (content.Info.Type == ContentType.Dir && content.ChildContent.Count > 0)
                {
                    var childTemplates = GetTemplateInfoForContents(content.ChildContent);
                    output.AddRange(childTemplates);
                }
                else if (content.Info.Name.EndsWith(Constants.TemplateFileType))
                {
                    var templateInfo = new TemplateGitInfo(content.Info, Repo);
                    output.Add(templateInfo);
                }
            }

            return output;
        }
    }
}
