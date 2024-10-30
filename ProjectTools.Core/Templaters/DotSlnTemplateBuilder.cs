using ProjectTools.Core.Constants;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

public class DotSlnTemplateBuilder()
    : AbstractTemplateBuilder(".sln", "A template builder for a .sln solutions.", "2.0.0", "Christian Webber")
{
    public override List<PreperationSlug> GetBaseSlugs(string pathToDirectoryToTemplate)
    {
        List<PreperationSlug> slugs = SlugConstants.GetBasePreparationSlugs();

        slugs.AddRange(
            [
                new PreperationSlug
                {
                    SlugKey = "Author",
                    DisplayName = "Author",
                    Type = SlugType.String,
                    RequiresUserInput = true,
                    SearchStrings = ["Author"]
                },
                new PreperationSlug
                {
                    SlugKey = "Description",
                    DisplayName = "Description",
                    Type = SlugType.String,
                    RequiresUserInput = true,
                    SearchStrings = ["Description"]
                },
                new PreperationSlug
                {
                    SlugKey = "Version",
                    DisplayName = "Version",
                    Type = SlugType.String,
                    RequiresUserInput = true,
                    SearchStrings = ["Version", "FileVersion", "AssemblyVersion"]
                }
            ]
        );

        return slugs;
    }

    public override void ReplaceSearchStringsWithSlugs(List<PreperationSlug> slugs, string pathToDirectoryToTemplate)
    {
        base.ReplaceSearchStringsWithSlugs(slugs, pathToDirectoryToTemplate);
    }

    public override bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate)
    {
        // check if any file is a .sln file...
        foreach (var file in Directory.GetFiles(pathToDirectoryToTemplate))
            if (file.EndsWith(".sln"))
                return true;

        foreach (var subDirectory in Directory.GetDirectories(pathToDirectoryToTemplate))
        {
            var result = IsValidDirectoryForBuilder(subDirectory);
            if (result) return result;
        }

        return false;
    }
}
