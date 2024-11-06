using ProjectTools.Core.Constants;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

public class DotSlnTemplateBuilder()
    : AbstractTemplateBuilder(".sln", "A template builder for a .sln solutions.", "2.0.0", "Christian Webber")
{
    public override List<PreparationSlug> GetPreparationSlugs(string pathToDirectoryToTemplate, Template template)
    {
        var slugs = SlugConstants.GetBasePreparationSlugs();

        slugs.AddRange(
            [
                new PreparationSlug
                {
                    SlugKey = "Author",
                    DisplayName = "Author",
                    Type = SlugType.String,
                    RequiresUserInput = true,
                    SearchStrings = ["Author"]
                },
                new PreparationSlug
                {
                    SlugKey = "Description",
                    DisplayName = "Description",
                    Type = SlugType.String,
                    RequiresUserInput = true,
                    SearchStrings = ["Description"]
                },
                new PreparationSlug
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
