using Octokit;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Settings;

namespace ProjectTools.Core.TemplateRepositories;

public class GitManager
{
    private readonly AppSettings? _appSettings;

    private readonly List<GitHubClient> _gitClients;

    private readonly Dictionary<string, int> _gitRepoToClientMapping;

    private readonly Dictionary<string, int> _gitSourcesToClientMapping;

    public GitManager()
    {
        this._gitClients = new List<GitHubClient>();
        this._gitSourcesToClientMapping = new Dictionary<string, int>();
        this._gitRepoToClientMapping = new Dictionary<string, int>();
        this._appSettings = AbstractSettings.Load();
        if (this._appSettings == null)
        {
            throw new Exception("App settings must be configured!");
        }
    }

    public GitHubClient? GetGitClientForGitSource(string source)
    {
        if (this._gitSourcesToClientMapping.TryGetValue(source, out var clientId))
        {
            return this._gitClients[clientId];
        }

        return AddGitClient(source);
    }

    /// <summary>
    /// </summary>
    /// <param name="repo"></param>
    /// <returns></returns>
    public GitHubClient? GetGitClientForRepo(string repo)
    {
        if (this._gitRepoToClientMapping.TryGetValue(repo, out var clientId))
        {
            return this._gitClients[clientId];
        }

        if (this._appSettings.RepositoriesList.TryGetValue(repo, out var source))
        {
            return GetGitClientForGitSource(source);
        }

        return null;
    }

    /// <summary>
    ///     Adds a git client to the manager
    /// </summary>
    /// <param name="source">The git source for the manager.</param>
    /// <returns>The git client if it could be created, or null.</returns>
    private GitHubClient? AddGitClient(string source)
    {
        // Step 1 - Get the client info if the source exists
        if (!this._appSettings.GitSources.TryGetValue(source, out var accessToken))
        {
            return null;
        }

        // Step 2 - Create and configure the client...
        var client = new GitHubClient(
            new ProductHeaderValue(AppConstants.AppName),
            new Uri(source)
        );
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var tokenAuth = new Credentials(accessToken);
            client.Credentials = tokenAuth;
        }

        // Step 3 - Grab all the repos for the source
        var repos = this._appSettings.RepositoriesList.Where(r => r.Value == source);

        // Step 4 - Add the client to the list and mappings
        var clientId = this._gitClients.Count;
        this._gitClients.Add(client);

        this._gitSourcesToClientMapping.Add(source, clientId);
        foreach (var repo in repos)
        {
            this._gitRepoToClientMapping.Add(repo.Key, clientId);
        }

        return client;
    }
}
