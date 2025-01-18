#region

using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Settings;
using ProjectTools.Core.TemplateRepositories;
using ProjectTools.Core.Templates;

#endregion

namespace ProjectTools.Core;

/// <summary>
///     A class to handle updating templates.
/// </summary>
public static class TemplateUpdater
{
    /// <summary>
    ///     A method to check for template updates.
    /// </summary>
    /// <param name="appSettings">The application settings.</param>
    /// <param name="forceCheck">True to force check for updates, False otherwise.</param>
    /// <param name="forceRedownload">True to force mark local templates as needing updates, False otherwise.</param>
    /// <returns>The update check results.</returns>
    public static async Task<TemplateUpdateCheckResult> AsyncCheckForTemplateUpdates(AppSettings? appSettings = null,
        bool forceCheck = false, bool forceRedownload = false)
    {
        var task = await Task.Run(() => CheckForTemplateUpdates(appSettings, forceCheck, forceRedownload));
        return task;
    }

    /// <summary>
    ///     A method to check for template updates.
    /// </summary>
    /// <param name="appSettings">The application settings.</param>
    /// <param name="forceCheck">True to force check for updates, False otherwise.</param>
    /// <param name="forceRedownload">True to force mark local templates as needing updates, False otherwise.</param>
    /// <returns>The update check results.</returns>
    public static TemplateUpdateCheckResult CheckForTemplateUpdates(AppSettings? appSettings = null,
        bool forceCheck = false, bool forceRedownload = false)
    {
        var result = new TemplateUpdateCheckResult();

        IOHelpers.CreateDirectoryIfNotExists(PathConstants.TemplateDirectory);
        appSettings ??= AbstractSettings.LoadOrThrow();

        // Exit early if we don't need to check for template updates...
        if (!forceCheck && !appSettings.ShouldUpdateTemplates)
        {
            return result;
        }

        // Get the status of any templates that we already have locally...
        var localTemplateManager = new LocalTemplates();
        var localTemplates = localTemplateManager.Templates;
        var localTemplateNames = localTemplates.Select(x => x.Name).ToList();

        // Get the remote templates...
        var remoteTemplateCollection = new RepositoryCollection();
        var remoteTemplates = remoteTemplateCollection.TemplateToTemplateGitMetadata;
        var remoteTemplateNames = remoteTemplates.Keys.ToList();
        result.TotalRemoteTemplatesProcessed = 0;

        // Return early if no remote templates were found...
        if (remoteTemplates.Count == 0)
        {
            result.OrphanedTemplates = localTemplates;
            return result;
        }

        // Otherwise, we need to see which local templates have remote ones and their respective versions...
        result.NewTemplates = remoteTemplates.Where(x => GetLocalTemplateByName(x.Key, localTemplates) == null)
            .Select(x => x.Value).ToList();
        result.UpdateableTemplates = remoteTemplates.Where(x =>
                localTemplateNames.Contains(x.Key) &&
                (forceRedownload || GetLocalTemplateByName(x.Key, localTemplates)?.Sha != x.Value.Sha))
            .Select(x => x.Value)
            .ToList();
        result.OrphanedTemplates = localTemplates.Where(x => !remoteTemplateNames.Contains(x.Name)).ToList();

        // Update that we've checked for updates and return update results!
        appSettings.LastTemplatesUpdateCheck = DateTime.Now;
        appSettings.Save();
        return result;
    }


    /// <summary>
    ///     Downloads the templates.
    /// </summary>
    /// <param name="templates">The templates to download.</param>
    /// <param name="force">True to force a download regardless of if the template exists already, False otherwise.</param>
    public static void DownloadTemplates(List<GitTemplateMetadata> templates,
        bool force = false)
    {
        IOHelpers.CreateDirectoryIfNotExists(PathConstants.TemplateDirectory);

        // Step 1 - Prep our new list of local templates...
        var localTemplateManager = new LocalTemplates();
        var newLocalTemplateInfo = localTemplateManager.Templates.Where(x => x.IsLocalOnlyTemplate).ToList();

        // Step 2 - Download the templates...
        foreach (var templateMetadata in templates)
        {
            // Step 2a - Check if we already have the template...
            var filePath = Path.Combine(PathConstants.TemplateDirectory, templateMetadata.SafeName);
            if (Path.GetExtension(filePath) != TemplateConstants.TemplateFileExtension)
            {
                filePath = $"{filePath}.{TemplateConstants.TemplateFileExtension}";
            }

            if (File.Exists(filePath))
            {
                if (!force)
                {
                    throw new Exception($"A template with the same name '{templateMetadata.Name}' already exists!");
                }

                IOHelpers.DeleteFileIfExists(filePath);
                if (newLocalTemplateInfo.Select(x => x.Name).Contains(templateMetadata.Name))
                {
                    newLocalTemplateInfo.Remove(newLocalTemplateInfo.First(x => x.Name == templateMetadata.Name));
                }
            }

            // Step 2b - Download the remote copy of the template...
            using (var client = new HttpClient())
            {
                using var s = client.GetStreamAsync(templateMetadata.Url);
                using var fs = new FileStream(filePath, FileMode.Create);
                s.Result.CopyTo(fs);
            }

            // Step 2c - Add the template to our new local template info list...
            var templateObj =
                JsonHelpers.DeserializeFromArchivedFile<Template>(filePath,
                    TemplateConstants.TemplateSettingsFileName);
            if (templateObj == null)
            {
                throw new Exception($"The downloaded template {templateMetadata.DisplayName} is invalid!");
            }

            newLocalTemplateInfo.Add(new LocalTemplateInfo(templateMetadata, filePath, templateObj));
        }

        // Step 3 - Update the template cache file with the new info...
        localTemplateManager.SaveLocalTemplateInfo(newLocalTemplateInfo);
    }

    /// <summary>
    ///     Grabs info on a local template by name.
    /// </summary>
    /// <param name="name">The name of the specific template.</param>
    /// <param name="localTemplates">The list of local templates.</param>
    /// <returns>The local template if it exists, or null.</returns>
    private static LocalTemplateInfo? GetLocalTemplateByName(string name, List<LocalTemplateInfo> localTemplates)
    {
        if (!localTemplates.Select(x => x.Name).Contains(name))
        {
            return null;
        }

        return localTemplates.First(x => x.Name == name);
    }
}
