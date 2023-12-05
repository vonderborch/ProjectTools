using System.Text.Json;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Core.Templating.Preparation;
using ProjectTools.Core.Templating.Repositories;

namespace ProjectTools.Core.Templating
{
    /// <summary>
    /// Class to handle templating logic (Generation and Preperation)
    /// </summary>
    public class Templater
    {
        /// <summary>
        /// The templates
        /// </summary>
        public Dictionary<string, AbstractTemplate> Templates;

        /// <summary>
        /// The settings
        /// </summary>
        private readonly Settings _settings;

        /// <summary>
        /// The template repositories collection
        /// </summary>
        private RepositoryCollection? _repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="Templater"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public Templater(Settings settings)
        {
            _settings = settings;
            _repositories = null;
            Templates = [];

            RefreshLocalTemplates();
        }

        /// <summary>
        /// Gets the repositories.
        /// </summary>
        /// <value>The repositories.</value>
        public RepositoryCollection Repositories
        {
            get
            {
                _repositories ??= new RepositoryCollection(_settings.TemplateRepositories, true);

                return _repositories;
            }
        }

        /// <summary>
        /// Gets the sorted template names.
        /// </summary>
        /// <value>The sorted template names.</value>
        public List<string> SortedTemplateNames => Templates.Keys.OrderBy(x => x).ToList();

        /// <summary>
        /// Gets the template preparer.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns></returns>
        public AbstractTemplatePreparer GetTemplatePreparer(Implementation implementation)
        {
            return TemplatePreperationFactory.GetTemplatePreparer(implementation);
        }

        /// <summary>
        /// Refreshes the local templates.
        /// </summary>
        public void RefreshLocalTemplates()
        {
            // read the local template cache
            if (!File.Exists(Constants.TemplatesCacheFile))
            {
                File.WriteAllText(Constants.TemplatesCacheFile, string.Empty);
            }

            List<TemplateGitMetadata> localTemplates = [];
            var fileContents = File.ReadAllText(Constants.TemplatesCacheFile);
            if (!string.IsNullOrWhiteSpace(fileContents))
            {
                localTemplates = JsonSerializer.Deserialize<List<TemplateGitMetadata>>(fileContents, Constants.JsonSerializeOptions) ?? [];
            }
            Templates.Clear();

            // load the local templates
            if (!Directory.Exists(Constants.TemplatesDirectory))
            {
                _ = Directory.CreateDirectory(Constants.TemplatesDirectory);
            }

            var templatesFiles = Directory.GetFiles(Constants.TemplatesDirectory, $"*.{Constants.TemplateFileType}", SearchOption.AllDirectories);
            templatesFiles ??= [];

            foreach (var file in templatesFiles)
            {
                var fileName = Path.GetFileName(file);

                var templateInfo = ArchiveHelpers.GetFileContentsFromArchive(file, Constants.TemplaterTemplatesInfoFileName);
                var template = JsonSerializer.Deserialize<AbstractTemplate>(templateInfo, Constants.JsonSerializeOptions);

                if (template != null)
                {
                    var gitInfo = localTemplates.FirstOrDefault(x => x.Name == fileName);
                    template.RepoInfo = gitInfo;
                    template.FilePath = file;
                    Templates.Add(template.Name, template);
                }
            }
        }
    }
}
