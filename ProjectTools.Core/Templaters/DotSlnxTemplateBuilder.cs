using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

public class DotSlnxTemplateBuilder()
    : AbstractTemplateBuilder(".slnx", "A template builder for a .slnx solutions.", "2.0.0", "Christian Webber")
{
    public override List<PreparationSlug> GetPreparationSlugsForTemplate(string pathToDirectoryToTemplate,
        PreparationTemplate template)
    {
        throw new NotImplementedException();
    }

    public override bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate)
    {
        // check if any file is a .sln file...
        foreach (var file in Directory.GetFiles(pathToDirectoryToTemplate))
        {
            if (file.EndsWith(".slnx"))
            {
                return true;
            }
        }

        foreach (var subDirectory in Directory.GetDirectories(pathToDirectoryToTemplate))
        {
            var result = IsValidDirectoryForBuilder(subDirectory);
            if (result)
            {
                return result;
            }
        }

        return false;
    }
}
