using Octokit;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Settings;

namespace ProjectTools.Core.TemplateRepositories;

/// <summary>
///     A manager for handling git clients
/// </summary>
public sealed class GitClientManager
{
    /// <summary>
    ///     The instance of this GitManager singleton
    /// </summary>
    private static readonly Lazy<GitClientManager> Lazy = new(() => new GitClientManager());

    /// <summary>
    ///     The git clients
    /// </summary>
    private readonly List<GitHubClient> _gitClients;

    /// <summary>
    ///     The mapping of git repos to clients
    /// </summary>
    private readonly Dictionary<string, int> _gitRepoToClientMapping;

    /// <summary>
    ///     The mapping of git sources to clients
    /// </summary>
    private readonly Dictionary<string, int> _gitSourcesToClientMapping;

    /// <summary>
    ///     Creates a new instance of the <see cref="GitClientManager" /> class.
    /// </summary>
    /// <exception cref="Exception">Raises if the app settings haven't been configured.</exception>
    private GitClientManager()
    {
        this._gitClients = new List<GitHubClient>();
        this._gitSourcesToClientMapping = new Dictionary<string, int>();
        this._gitRepoToClientMapping = new Dictionary<string, int>();
    }

    public static GitClientManager ClientManager => Lazy.Value;

    /// <summary>
    ///     Gets the git client for a git source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="appSettings">The application settings.</param>
    /// <returns>The git client for the source, or null.</returns>
    public GitHubClient? GetGitClientForGitSource(string source, AppSettings? appSettings = null)
    {
        if (this._gitSourcesToClientMapping.TryGetValue(source, out var clientId))
        {
            return this._gitClients[clientId];
        }

        return AddGitClient(source, appSettings);
    }

    /// <summary>
    ///     Gets the git client for a repo.
    /// </summary>
    /// <param name="repo">The repo.</param>
    /// <param name="appSettings">The application settings.</param>
    /// <returns>The git client for the repo, or null.</returns>
    public GitHubClient? GetGitClientForRepo(string repo, AppSettings? appSettings = null)
    {
        if (this._gitRepoToClientMapping.TryGetValue(repo, out var clientId))
        {
            return this._gitClients[clientId];
        }

        appSettings ??= AbstractSettings.LoadOrThrow();
        if (appSettings.RepositoriesAndGitSources.TryGetValue(repo, out var source))
        {
            return GetGitClientForGitSource(source, appSettings);
        }

        return null;
    }

    /// <summary>
    ///     Adds a git client to the manager
    /// </summary>
    /// <param name="source">The git source for the manager.</param>
    /// <param name="appSettings">The app settings.</param>
    /// <returns>The git client if it could be created, or null.</returns>
    private GitHubClient? AddGitClient(string source, AppSettings? appSettings = null)
    {
        // Step 1 - Get the client info if the source exists
        appSettings ??= AbstractSettings.LoadOrThrow();
        if (!appSettings.GitSourcesAndAccessTokens.TryGetValue(source, out var accessToken))
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
        var repos = appSettings.RepositoriesAndGitSources.Where(r => r.Value == source);

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
