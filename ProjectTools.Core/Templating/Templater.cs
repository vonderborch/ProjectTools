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
                _repositories ??= new RepositoryCollection(_settings.TemplateRepositories, false);

                return _repositories;
            }
        }

        /// <summary>
        /// Gets the sorted template names.
        /// </summary>
        /// <value>The sorted template names.</value>
        public List<string> SortedTemplateNames => Templates.Keys.OrderBy(x => x).ToList();

        /// <summary>
        /// Checks for template updates.
        /// </summary>
        /// <param name="forceUpdate">if set to <c>true</c> [force update].</param>
        /// <returns>
        /// Item 1 - total found templates, Item 2 - new templates, Item 3 - templates to update, Item 4 - orphaned templates
        /// </returns>
        public (int, List<TemplateGitMetadata>, List<TemplateGitMetadata>, List<string>) CheckForTemplateUpdates(bool forceUpdate = false)
        {
            // Check if templates directory exists
            if (!Directory.Exists(Constants.TemplatesDirectory))
            {
                _ = Directory.CreateDirectory(Constants.TemplatesDirectory);
            }

            if (!forceUpdate && !Manager.Instance.Settings.ShouldUpdateTemplates)
            {
                return (-1, [], [], []);
            }

            // Grab the remote and local templates
            var remoteTemplates = Repositories.GetTemplateInfoForRepositories(true);
            RefreshLocalTemplates();

            // Determine which templates we need to download
            var newTemplates = remoteTemplates.Where(x => !Templates.ContainsKey(x.DisplayName)).ToList();
            var updateableTemplates = remoteTemplates.Where(x => Templates.ContainsKey(x.DisplayName) && (Templates[x.DisplayName].RepoInfo?.SHA != x.SHA)).ToList();
            var orphanedTemplates = Templates.Keys.Where(x => !remoteTemplates.Any(y => y.DisplayName == x)).ToList();

            // update settings and return
            Manager.Instance.Settings.LastTemplatesUpdateCheck = DateTime.Now;
            Manager.Instance.Settings.SaveFile(Constants.SettingsFile);
            return (remoteTemplates.Count, newTemplates, updateableTemplates, orphanedTemplates);
        }

        /// <summary>
        /// Detects the valid implementation.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>A valid implementation for the specified directory, or null if none is found.</returns>
        public Implementation? DetectValidImplementation(string directory)
        {
            foreach (var imp in Enum.GetValues(typeof(Implementation)))
            {
                var implementation = (Implementation)imp;

                var preparer = TemplatePreperationFactory.GetTemplatePreparer(implementation);
                if (preparer.DirectoryValidForTemplater(directory))
                {
                    return implementation;
                }
            }

            return null;
        }

        /// <summary>
        /// Downloads the templates.
        /// </summary>
        /// <param name="templates">The templates.</param>
        public void DownloadTemplates(List<TemplateGitMetadata> templates)
        {
            RefreshLocalTemplates();

            var updatedNames = templates.Select(x => x.Name).ToList();
            var newTemplateCache = Templates.Values.Where(x => updatedNames.Contains(x.Name)).Select(x => x.RepoInfo).ToList();

            foreach (var template in templates)
            {
                // delete the local copy of the template if it exists
                if (Templates.ContainsKey(template.Name))
                {
                    var localInfo = Templates[template.Name];
                    if (File.Exists(localInfo.FilePath))
                    {
                        File.Delete(localInfo.FilePath);
                    }
                }

                // Update template cache
                newTemplateCache.Add(template);

                // download the remote copy of the template
                var filePath = Path.Combine(Constants.TemplatesDirectory, template.Name);

                using var client = new HttpClient();
                using var s = client.GetStreamAsync(template.Url);
                using var fs = new FileStream(filePath, FileMode.Create);
                s.Result.CopyTo(fs);
            }

            // update the template cache file
            var newFileContents = JsonSerializer.Serialize(newTemplateCache, Constants.JsonSerializeOptions);
            File.WriteAllText(Constants.TemplatesCacheFile, newFileContents);
            RefreshLocalTemplates();
        }

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

            // update the in-memory cache of templates
            var templatesFiles = Directory.GetFiles(Constants.TemplatesDirectory, $"*.{Constants.TemplateFileType}", SearchOption.AllDirectories);
            templatesFiles ??= [];
            List<TemplateGitMetadata> newTemplateCache = [];
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

                    newTemplateCache.Add(gitInfo ?? new TemplateGitMetadata() { Name = template.Name });
                }
            }

            // Update the local template cache file
            var newFileContents = JsonSerializer.Serialize(newTemplateCache, Constants.JsonSerializeOptions);
            File.WriteAllText(Constants.TemplatesCacheFile, newFileContents);
        }
    }
}
