using System.Diagnostics;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Core.Templating.Repositories;

namespace ProjectTools.Core.Templating
{
    /// <summary>
    /// A manager for templates, remote or local
    /// </summary>
    [DebuggerDisplay("{_localTemplates.Count} | {_remoteRepositories}")]
    public class TemplateManager
    {
        /// <summary>
        /// The local templates
        /// </summary>
        private readonly Dictionary<string, MetaTemplate> _localTemplates;

        /// <summary>
        /// The remote repositories
        /// </summary>
        private readonly RepositoryCollection? _remoteRepositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateManager"/> class.
        /// </summary>
        public TemplateManager()
        {
            _remoteRepositories = new RepositoryCollection(Manager.Instance.Settings.TemplateRepositories, false);
            _localTemplates = [];

            RefreshLocalTemplates();
        }

        /// <summary>
        /// Gets the sorted local template names.
        /// </summary>
        /// <value>The sorted local template names.</value>
        public List<string> SortedLocalTemplateNames => _localTemplates.Keys.Cast<string>().Order().ToList();

        /// <summary>
        /// Checks for template updates.
        /// </summary>
        /// <param name="forceUpdates">if set to <c>true</c> [force redownload all templates].</param>
        /// <param name="ignoreCache">if set to <c>true</c> [override the update check].</param>
        /// <param name="autoUpdate">if set to <c>true</c> [auto update].</param>
        /// <returns>The results of the update check</returns>
        public TemplateUpdateResult CheckForTemplateUpdates(bool forceUpdates = false, bool ignoreCache = false, bool autoUpdate = false)
        {
            IOHelpers.CreateDirectoryIfNotExists(Constants.TemplatesDirectory);

            // Exit early if we shouldn't bother checking for updates...
            if (!ignoreCache && !Manager.Instance.Settings.ShouldUpdateTemplates)
            {
                return new TemplateUpdateResult(-1, [], [], []);
            }

            // Grab the latest remote and local templates
            RefreshLocalTemplates();
            var remoteTemplates = _remoteRepositories?.GetTemplateInfoForRepositories(true);

            if (remoteTemplates == null || remoteTemplates.Count == 0)
            {
                // return early if no remote templates were found
                var localTemplateNames = _localTemplates.Keys.Cast<string>().ToList();
                return new TemplateUpdateResult(0, [], [], localTemplateNames);
            }
            else
            {
                // Determine the state of the local templates and remote templates...
                var localTemplates = _localTemplates.Values.ToList();
                var localTemplateNames = localTemplates.Select(x => x.Template.Information.Name).ToList();

                var newTemplates = remoteTemplates.Where(x => !localTemplateNames.Contains(x.DisplayName)).ToList();
                var updateableTemplates = remoteTemplates.Where(x => localTemplateNames.Contains(x.DisplayName) && (GetTemplateByName(x.DisplayName)?.RepoInfo?.SHA != x.SHA || forceUpdates)).ToList();
                var orphanedTemplates = localTemplateNames.Where(x => !remoteTemplates.Any(y => y.DisplayName == x)).ToList();

                // update settings and return
                Manager.Instance.Settings.LastTemplatesUpdateCheck = DateTime.Now;
                Manager.Instance.Settings.SaveFile(Constants.SettingsFile);
                if (autoUpdate)
                {
                    var templatesToDownload = new List<TemplateGitMetadata>();
                    templatesToDownload.AddRange(newTemplates);
                    templatesToDownload.AddRange(updateableTemplates);
                    DownloadTemplates(templatesToDownload);
                }
                return new TemplateUpdateResult(remoteTemplates?.Count ?? -1, newTemplates, updateableTemplates, orphanedTemplates);
            }
        }

        /// <summary>
        /// Detects the valid implementation for directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>The first valid implementation for the directory, or null if none detected.</returns>
        public Implementation? DetectValidImplementationForDirectory(string directory)
        {
            foreach (var implementation in TemplaterFactory.GetImplementations())
            {
                var templater = TemplaterFactory.GetTemplater(implementation);
                if (templater.DirectoryValidForTemplatePreperation(directory))
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
            IOHelpers.CreateDirectoryIfNotExists(Constants.TemplatesDirectory);
            RefreshLocalTemplates();

            var updatingNames = templates.Select(x => x.DisplayName).ToList();

            // Initialize the new template cache to any template meta info for templates that aren't being updated
            var newTemplateCache = _localTemplates.Values.Cast<MetaTemplate>().Where(x => !updatingNames.Contains(x.Template.Information.Name)).Select(x => x.RepoInfo).ToList();
            var updatingTemplates = _localTemplates.Values.Cast<MetaTemplate>().Where(x => updatingNames.Contains(x.Template.Information.Name)).ToDictionary(x => x.Template.Information.Name, y => y);

            newTemplateCache ??= [];
            updatingTemplates ??= [];

            // Download the templates
            foreach (var template in templates)
            {
                // Delete the local copy of the template (if it exists)
                if (updatingTemplates.TryGetValue(template.DisplayName, out var localTemplate))
                {
                    IOHelpers.DeleteFileIfExists(localTemplate.FilePath);
                }

                // Add the updated info to the new template cache
                newTemplateCache.Add(template);

                // Download the remote copy of the template
                var filePath = Path.Combine(Constants.TemplatesDirectory, template.Name);

                using var client = new HttpClient();
                using var s = client.GetStreamAsync(template.Url);
                using var fs = new FileStream(filePath, FileMode.Create);
                s.Result.CopyTo(fs);
            }

            // Update the template cache file and refresh our local templates
            JsonHelpers.WriteObjectToFile(newTemplateCache, Constants.TemplatesCacheFile);
            RefreshLocalTemplates();
        }

        /// <summary>
        /// Gets the abstract templater for implementation.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns>The templater for the specified implementation.</returns>
        public AbstractTemplater GetAbstractTemplaterForImplementation(Implementation implementation)
        {
            return TemplaterFactory.GetTemplater(implementation);
        }

        /// <summary>
        /// Gets the template for the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The template.</returns>
        /// <exception cref="ArgumentOutOfRangeException">nameof(name), $"No template found for name {name}</exception>
        public MetaTemplate GetTemplateByName(string name)
        {
            return _localTemplates.TryGetValue(name, out var template)
                ? template
                : throw new ArgumentOutOfRangeException(nameof(name), $"No template found for name {name}");
        }

        /// <summary>
        /// Gets the templater for template.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The templater for the template.</returns>
        public AbstractTemplater GetTemplaterForTemplate(string name)
        {
            var template = GetTemplateByName(name);
            var templater = TemplaterFactory.GetTemplater(template.Implementation);
            return templater;
        }

        /// <summary>
        /// Gets the templater for template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>The templater for the template.</returns>
        public AbstractTemplater GetTemplaterForTemplate(MetaTemplate template)
        {
            var templater = TemplaterFactory.GetTemplater(template.Implementation);
            return templater;
        }

        /// <summary>
        /// Refreshes the local templates cache.
        /// </summary>
        public void RefreshLocalTemplates()
        {
            IOHelpers.CreateDirectoryIfNotExists(Constants.TemplatesDirectory);
            _localTemplates.Clear();

            // Load the template git metadata from the cache file (this is used to determine if we need to update any template)
            var gitTemplateMetadata = GetTemplateCacheInfo();

            // find all local templates
            var templatesFiles = Directory.GetFiles(Constants.TemplatesDirectory, $"*.{Constants.TemplateFileType}", SearchOption.AllDirectories);

            if (templatesFiles.Length == 0)
            {
                // return early if no templates were found
                return;
            }

            var templaters = TemplaterFactory.GetAllRegisteredTemplaters();

            // load the templates into memory
            foreach (var file in templatesFiles)
            {
                var fileName = Path.GetFileName(file);
                try
                {
                    var templateCoreInfo = JsonHelpers.DeserializeArchivedFile<Template>(file, Constants.TemplaterTemplatesInfoFileName);
                    if (templateCoreInfo == null)
                    {
                        // continue if the template is invalid
                        // TODO: In the future we should probably surface this to the user somehow...
                        continue;
                    }

                    var gitInfo = gitTemplateMetadata.FirstOrDefault(x => x.Name == fileName);
                    AbstractTemplater? validTemplater = null;
                    foreach (var templater in templaters)
                    {
                        if (templater.TemplaterValidForTemplate(templateCoreInfo))
                        {
                            validTemplater = templater;
                            break;
                        }
                    }
                    if (validTemplater == null)
                    {
                        throw new Exception($"No valid templater found for template {fileName}");
                    }

                    var template = new MetaTemplate() { FilePath = file, RepoInfo = gitInfo, Template = templateCoreInfo, Implementation = validTemplater.Implementation };
                    _localTemplates.Add(template.Template.Information.Name, template);
                }
                catch
                {
                    // If we run into any errors, it's an invalid template, so we should just continue (and delete the
                    // local copy!)
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Templates the exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>True if the template exists, False otherwise.</returns>
        public bool TemplateExists(string name)
        {
            return _localTemplates.ContainsKey(name);
        }

        /// <summary>
        /// Gets the template cache information.
        /// </summary>
        /// <returns>A list of git metadata for the local templates (stored in the cache file)</returns>
        private static List<TemplateGitMetadata> GetTemplateCacheInfo()
        {
            IOHelpers.CreateFileIfNotExists(Constants.TemplatesCacheFile);
            var output = JsonHelpers.DeserializeFile<List<TemplateGitMetadata>>(Constants.TemplatesCacheFile) ?? [];

            return output;
        }
    }
}
