using ProjectTools.Core.Internal.Configuration;
using ProjectTools.Core.Internal.Implementations;
using ProjectTools.Core.Internal.Repositories;
using System.Collections.Generic;
using System.Text.Json;

namespace ProjectTools.Core.Internal
{
    public class Templater
    {
        private RepositoryCollection? _repositories;

        private Settings _settings;

        /// <summary>
        /// The template unique identifier counts
        /// </summary>
        public Dictionary<string, int> TemplateGuidCounts = new();

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

        public string Prepare(
            PrepareOptions options,
            TemplaterImplementations implementation,
            Func<string, bool> log
        )
        {
            var templater = TemplateFactory.GetTemplaterForImplementation(implementation);
            return templater.Prepare(options, log);
        }

        public void RefreshLocalTemplatesList()
        {
            List<TemplateGitInfo> localTemplates = new();

            // load the local templatesinfo file to see current template status
            if (File.Exists(Constants.TemplatesCacheFile))
            {
                var localTemplatesFromCache = JsonSerializer.Deserialize<List<TemplateGitInfo>>(
                    File.ReadAllText(Constants.TemplatesCacheFile)
                );
                if (localTemplatesFromCache != null)
                {
                    localTemplates = localTemplatesFromCache;
                }
            }

            LocalTemplates.Clear();

            // create the templates directory if it doesn't exist
            if (!Directory.Exists(Constants.TemplatesDirectory))
            {
                Directory.CreateDirectory(Constants.TemplatesDirectory);
            }

            // go through each template in the templates directory...
            var templates = Directory.GetFiles(
                Constants.TemplatesDirectory,
                $"*.{Constants.TemplateFileType}"
            );
            if (templates == null)
            {
                return;
            }

            foreach (var file in templates)
            {
                var fileName = Path.GetFileName(file);
                var localTemplate = localTemplates.FirstOrDefault(t => t.Name == fileName);
                var repoMeta = localTemplate;
                var template = new Template(file, repoMeta);
                TemplateGuidCounts[template.Name] = GetGuidCount(file);

                LocalTemplates.Add(template);
            }
        }
    }
}
