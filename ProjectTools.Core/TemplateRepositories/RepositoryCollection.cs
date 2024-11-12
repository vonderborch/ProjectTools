using System.Diagnostics;
using System.Text.Json.Serialization;
using ProjectTools.Core.Settings;

namespace ProjectTools.Core.TemplateRepositories;

/// <summary>
///     Represents a collection of repositories.
/// </summary>
[DebuggerDisplay("{Repositories.Count}")]
public class RepositoryCollection
{
    /// <summary>
    ///     The cache of git template metadata.
    /// </summary>
    private Dictionary<string, GitTemplateMetadata> _templateMapCache = [];

    /// <summary>
    ///     Creates a new instance of the <see cref="RepositoryCollection" /> class.
    /// </summary>
    /// <param name="autoLoad">True to autoload, False otherwise.</param>
    public RepositoryCollection(bool autoLoad = true)
    {
        // Load the app settings and gather all repos
        var appSettings = AbstractSettings.LoadOrThrow();
        var repos = appSettings.RepositoriesList;

        // Load all repos
        this.Repositories = (from repo in repos select new Repository(repo, autoLoad)).ToList();
    }

    /// <summary>
    ///     The repositories.
    /// </summary>
    public List<Repository> Repositories { get; }

    /// <summary>
    ///     A list of repository names handled by this repository collection.
    /// </summary>
    public List<string> RepositoryNames => this.Repositories.Select(x => x.Repo).ToList();

    /// <summary>
    ///     A dictionary representing the template git metadata, keyed by template name
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, GitTemplateMetadata> TemplateToTemplateGitMetadata
    {
        get
        {
            if (this._templateMapCache.Count == 0)
            {
                GetTemplateInfoForRepositories();
            }

            return this._templateMapCache;
        }
    }

    /// <summary>
    ///     Gets the template information for repository.
    /// </summary>
    /// <param name="forceLoad">if set to <c>true</c> [force load].</param>
    /// <returns>Template info for the repository</returns>
    public Dictionary<string, GitTemplateMetadata> GetTemplateInfoForRepositories(bool forceLoad = true)
    {
        var output = new Dictionary<string, GitTemplateMetadata>();
        foreach (var repo in this.Repositories)
        {
            var templates = repo.GetTemplateInfoForRepository(forceLoad);
            foreach (var template in templates)
            {
                output[template.Name] = template;
            }
        }

        this._templateMapCache = output;
        return output;
    }
}
