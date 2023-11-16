using ProjectTools.Core.Internal.Configuration;
using ProjectTools.Core.Internal.Implementations;
using ProjectTools.Core.Internal.Repositories;
using System.Text.Json;

namespace ProjectTools.Core.Internal
{
    public class Templater
    {
        private RepositoryCollection? _repositories;

        private Settings _settings;

        public Templater(Settings settings)
        {
            _settings = settings;
        }

        public List<Template> LocalTemplates { get; private set; }

        public RepositoryCollection Repositories
        {
            get
            {
                if (_repositories == null)
                {
                    _repositories = new(_settings.TemplateRepositories, true);
                }

                return _repositories;
            }
        }

        public SolutionSettingsFactory SolutionSettingsFactory { get; } = new();

        public TemplateFactory TemplateFactory { get; } = new();

        public string Prepare(PrepareOptions options, TemplaterImplementations implementation, Func<string, bool> log)
        {
            var templater = TemplateFactory.GetTemplaterForImplementation(implementation);
            return templater.Prepare(options, log);
        }

        public void RefreshLocalTemplatesList()
        {
            var localTemplates = JsonSerializer.Deserialize<List<TemplateGitInfo>>(File.ReadAllText(Constants.TemplatesCacheFile));
            if (localTemplates == null)
            {
                localTemplates = new();
            }
        }
    }
}
