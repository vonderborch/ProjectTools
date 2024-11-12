using System.Diagnostics;
using Octokit;
using ProjectTools.Core.Constants;

namespace ProjectTools.Core.TemplateRepositories;

/// <summary>
///     A class representing a repository to look for templates in.
/// </summary>
[DebuggerDisplay("{Repo} | {Templates.Count}")]
public class Repository
{
    /// <summary>
    ///     Creates a new instance of the <see cref="Repository" /> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    /// <param name="autoLoad">True to search for templates, False otherwise.</param>
    public Repository(string repository, bool autoLoad = true)
    {
        this.Repo = repository;
        var splitRepositoryName = repository.Split('/');
        this.RepositoryOwner = splitRepositoryName[^2];
        this.RepositoryName = splitRepositoryName[^1];

        this.Templates = [];
        if (autoLoad)
        {
            GetTemplateInfoForRepository();
        }
    }

    /// <summary>
    ///     The repository
    /// </summary>
    public string Repo { get; }

    /// <summary>
    ///     The name of the repository
    /// </summary>
    public string RepositoryName { get; }

    /// <summary>
    ///     The owner of the repository
    /// </summary>
    public string RepositoryOwner { get; }

    /// <summary>
    ///     The list of templates in the repository
    /// </summary>
    public List<GitTemplateMetadata> Templates { get; private set; }

    /// <summary>
    ///     A dictionary representing the templates, keyed by template name
    /// </summary>
    public Dictionary<string, GitTemplateMetadata> TemplateMap => this.Templates.ToDictionary(t => t.Name, t => t);


    /// <summary>
    ///     Gets information on templates stored in the repository this instance represents.
    /// </summary>
    /// <param name="forceLoad">
    ///     True to scan the repository to templates, false to not scan the repository if we already have templates
    ///     loaded. Defaults to true.
    /// </param>
    /// <returns>A list of templates discovered in the repository.</returns>
    public List<GitTemplateMetadata> GetTemplateInfoForRepository(bool forceLoad = true)
    {
        if (forceLoad || this.Templates.Count == 0)
        {
            var client = GitClientManager.ClientManager.GetGitClientForRepo(this.Repo);
            if (client == null)
            {
                throw new Exception("Could not get client for repo!");
            }

            var rootContents = client.Repository.Content
                .GetAllContents(this.RepositoryOwner, this.RepositoryName).Result.ToList();
            var repoContents = rootContents
                .Select(x => new GitRepoContents(client, x, this.RepositoryOwner, this.RepositoryName, x.Path))
                .ToList();

            this.Templates = GetTemplateInfoForContents(repoContents);
        }

        return this.Templates;
    }

    /// <summary>
    ///     Checks git contents for templates.
    /// </summary>
    /// <param name="contents">The contents to check for templates</param>
    /// <returns>A list of templates the git contents store</returns>
    private List<GitTemplateMetadata> GetTemplateInfoForContents(List<GitRepoContents> contents)
    {
        var output = new List<GitTemplateMetadata>();

        foreach (var content in contents)
        {
            if (content.Info.Type == ContentType.Dir && content.ChildContent.Count > 0)
            {
                var childTemplates = GetTemplateInfoForContents(content.ChildContent);
                output.AddRange(childTemplates);
            }
            else if (content.Info.Name.EndsWith(TemplateConstants.TemplateFileExtension))
            {
                var templateInfo = new GitTemplateMetadata(content.Info, this.Repo);
                output.Add(templateInfo);
            }
        }

        return output;
    }
}
