using Octokit;
using ProjectTools.Core.Template.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTools.Core
{
    public sealed class Manager
    {
        private static readonly Lazy<Manager> _lazy = new Lazy<Manager>(() => new Manager());

        private Template.Repositories.RepositoryCollection? _repositories;

        private Settings? _settings;

        private GitHubClient? _gitClient;

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

        public Template.Repositories.RepositoryCollection Repositories
        {
            get
            {
                if (_repositories == null)
                {
                    _repositories = new(Settings.TemplateRepositories, true);
                }

                return _repositories;
            }
        }

        public bool ValidateSettings()
        {
            return File.Exists(Constants.SettingsFile);
        }
    }
}
