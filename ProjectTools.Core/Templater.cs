using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core;

/// <summary>
/// </summary>
public class Templater
{
    public List<LocalTemplateInfo> LocalTemplates;

    public Templater()
    {
        this.LocalTemplates =
            JsonHelpers.DeserializeFromFile<List<LocalTemplateInfo>>(PathConstants.TemplatesInfoCacheFile) ?? [];
    }

    public List<LocalTemplateInfo> PopulateLocalTemplates(bool forceRefresh = false)
    {
        if (this.LocalTemplates.Count > 0 && !forceRefresh)
        {
            return this.LocalTemplates;
        }

        this.LocalTemplates = [];
        var existingInfo =
            JsonHelpers.DeserializeFromFile<List<LocalTemplateInfo>>(PathConstants.TemplatesInfoCacheFile) ?? [];

        this.LocalTemplates = [];
        // Find all template files and load the template.json file from them into memory!
        IOHelpers.CreateDirectoryIfNotExists(PathConstants.TemplateDirectory);
        var templateFiles = Directory.GetFiles(PathConstants.TemplateDirectory,
            $"*.{TemplateConstants.TemplateFileExtension}", SearchOption.AllDirectories);
        foreach (var templateFile in templateFiles)
        {
            var fileName = Path.GetFileName(templateFile);

            try
            {
                var template =
                    JsonHelpers.DeserializeFromArchivedFile<Template>(templateFile,
                        TemplateConstants.TemplateSettingsFileName);
                if (template == null)
                {
                    throw new Exception("Invalid template file!");
                }

                var existingInfoForTemplate = existingInfo.FirstOrDefault(x => x.Name == template.Name);
                if (existingInfoForTemplate != null)
                {
                    existingInfoForTemplate.Template = template;
                    this.LocalTemplates.Add(existingInfoForTemplate);
                }
                else
                {
                    // First time we've seen this file!
                    var info = new LocalTemplateInfo
                    {
                        LocalPath = templateFile,
                        Name = template.Name,
                        Template = template,
                        Size = (ulong)new FileInfo(templateFile).Length
                    };
                    this.LocalTemplates.Add(info);
                }
            }
            catch
            {
                // TODO: Do something with the error?
            }
        }

        return this.LocalTemplates;
    }

    public void UpdateTemplates()
    {
    }
}
