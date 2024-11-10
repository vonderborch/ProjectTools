using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core;

/// <summary>
///     A class that manages local templates.
/// </summary>
public class LocalTemplates
{
    /// <summary>
    ///     The list of local templates...
    /// </summary>
    private List<LocalTemplateInfo> _templates;

    /// <summary>
    ///     Creates a new instance of the <see cref="LocalTemplates" /> class.
    /// </summary>
    public LocalTemplates()
    {
        IOHelpers.CreateDirectoryIfNotExists(PathConstants.TemplateDirectory);

        this._templates = new List<LocalTemplateInfo>();
        PopulateLocalTemplates(true);
    }

    /// <summary>
    ///     The list of local templates.
    /// </summary>
    public List<LocalTemplateInfo> Templates => [..this._templates];


    public List<LocalTemplateInfo> PopulateLocalTemplates(bool forceRefresh = false)
    {
        if (this._templates.Count > 0 && !forceRefresh)
        {
            return this._templates;
        }

        this._templates = [];
        var existingInfo =
            JsonHelpers.DeserializeFromFile<List<LocalTemplateInfo>>(PathConstants.TemplatesInfoCacheFile) ?? [];

        this._templates = [];
        // Find all template files and load the template.json file from them into memory!
        var templateFiles = Directory.GetFiles(PathConstants.TemplateDirectory,
            $"*.{TemplateConstants.TemplateFileExtension}", SearchOption.AllDirectories);
        foreach (var templateFile in templateFiles)
        {
            try
            {
                var template =
                    JsonHelpers.DeserializeFromArchivedFile<Template>(templateFile,
                        TemplateConstants.TemplateSettingsFileName);
                if (template == null)
                {
                    throw new Exception("Invalid template file!");
                }

                try
                {
                    var existingInfoForTemplate = existingInfo.First(x => x.Name == template.Name);
                    existingInfoForTemplate.Template = template;
                    this._templates.Add(existingInfoForTemplate);
                }
                catch (ArgumentNullException)
                {
                    // First time we've seen this file!
                    var info = new LocalTemplateInfo
                    {
                        LocalPath = templateFile,
                        Name = template.Name,
                        Template = template,
                        Size = (ulong)new FileInfo(templateFile).Length
                    };
                    this._templates.Add(info);
                }
            }
            catch
            {
                // TODO: Do something with the error?
            }
        }

        return this._templates;
    }
}
