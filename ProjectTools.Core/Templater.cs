using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core;

/// <summary>
/// 
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
        IOHelpers.CreateDirectoryIfNotExists(PathConstants.TemplateDirectory);

        // Find all template files and load the template.json file from them into memory!
        var templateFiles = Directory.GetFiles(PathConstants.TemplateDirectory,
            $"*.{TemplateConstants.TemplateFileExtension}", SearchOption.AllDirectories);
        foreach (var templateFile in templateFiles)
        {
            //
        }

        return this.LocalTemplates;
    }

    public void UpdateTemplates()
    {
    }
}
