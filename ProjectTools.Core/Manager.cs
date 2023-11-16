using Octokit;
using ProjectTools.Core.Internal;

namespace ProjectTools.Core
{
    public sealed class Manager
    {
        private static readonly Lazy<Manager> _lazy = new Lazy<Manager>(() => new Manager());

        private GitHubClient? _gitClient;
        private Settings? _settings;

        private Templater? _templater;

        private Manager()
        {
            _settings = null;
        }

        public static Manager Instance => _lazy.Value;

        public GitHubClient GitClient
        {
            get
            {
                if (_gitClient == null)
                {
                    _gitClient = new GitHubClient(new ProductHeaderValue(Constants.AppName), new Uri(Settings.GitWebPath));

                    if (!string.IsNullOrWhiteSpace(Settings.GitAccessToken))
                    {
                        var tokenAuth = new Credentials(Settings.GitAccessToken);
                        _gitClient.Credentials = tokenAuth;
                    }
                }

                return _gitClient;
            }
        }

        public Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Settings.LoadFile(Constants.SettingsFile);
                    if (_settings == null)
                    {
                        _settings = new();
                    }
                }
                return _settings;
            }
        }

        public Templater Templater
        {
            get
            {
                if (_templater == null)
                {
                    _templater = new(Settings);
                }
                return _templater;
            }
        }

        public bool ValidateSettings()
        {
            return File.Exists(Constants.SettingsFile);
        }
    }
}
