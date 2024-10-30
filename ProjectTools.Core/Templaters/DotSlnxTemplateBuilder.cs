using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

public class DotSlnxTemplateBuilder()
    : AbstractTemplateBuilder(".slnx", "A template builder for a .slnx solutions.", "2.0.0", "Christian Webber")
{
    public override List<PreperationSlug> GetBaseSlugs(string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }

    public override void ReplaceSearchStringsWithSlugs(List<PreperationSlug> slugs, string pathToDirectoryToTemplate)
    {
        base.ReplaceSearchStringsWithSlugs(slugs, pathToDirectoryToTemplate);
    }

    public override bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate)
    {
        // check if any file is a .sln file...
        foreach (var file in Directory.GetFiles(pathToDirectoryToTemplate))
            if (file.EndsWith(".slnx"))
                return true;

        foreach (var subDirectory in Directory.GetDirectories(pathToDirectoryToTemplate))
        {
            var result = IsValidDirectoryForBuilder(subDirectory);
            if (result) return result;
        }

        return false;
    }
}
