using Octokit;
using ProjectTools.Core.Internal;
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

        private Settings? _settings;

        private Templater? _templater;

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
