using Octokit;
using ProjectTools.Core.Templating;

namespace ProjectTools.Core
{
    /// <summary>
    /// The core of the ProjectTools library
    /// </summary>
    public sealed class Manager
    {
        /// <summary>
        /// The instance of this Manager singleton
        /// </summary>
        private static readonly Lazy<Manager> _lazy = new(() => new Manager());

        /// <summary>
        /// The git client
        /// </summary>
        private GitHubClient? _gitClient;

        /// <summary>
        /// The settings
        /// </summary>
        private Settings? _settings;

        /// <summary>
        /// The templater
        /// </summary>
        private TemplateManager? _templater;

        /// <summary>
        /// Prevents a default instance of the <see cref="Manager"/> class from being created.
        /// </summary>
        private Manager()
        {
            _settings = null;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Manager Instance => _lazy.Value;

        /// <summary>
        /// Gets the git client.
        /// </summary>
        /// <value>The git client.</value>
        public GitHubClient GitClient
        {
            get
            {
                if (_gitClient == null)
                {
                    _gitClient = new GitHubClient(
                        new ProductHeaderValue(Constants.ApplicationName),
                        new Uri(Settings.GitWebPath)
                                                 );

                    if (!string.IsNullOrWhiteSpace(Settings.GitAccessToken))
                    {
                        var tokenAuth = new Credentials(Settings.GitAccessToken);
                        _gitClient.Credentials = tokenAuth;
                    }
                }

                return _gitClient;
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Settings.LoadFile(Constants.SettingsFile);
                    _settings ??= new();
                }
                return _settings;
            }
        }

        /// <summary>
        /// Gets the templater.
        /// </summary>
        /// <value>The templater.</value>
        public TemplateManager Templater
        {
            get
            {
                _templater ??= new();
                return _templater;
            }
        }

        /// <summary>
        /// Validates the settings.
        /// </summary>
        /// <returns></returns>
        public bool ValidateSettings()
        {
            return File.Exists(Constants.SettingsFile);
        }
    }
}
