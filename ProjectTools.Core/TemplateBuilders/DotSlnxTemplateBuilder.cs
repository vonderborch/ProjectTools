using ProjectTools.Core.Templates;

namespace ProjectTools.Core.TemplateBuilders;

public class DotSlnxTemplateBuilder()
    : AbstractTemplateBuilder(".slnx", "A template builder for a .slnx solutions.", "2.0.0", "Christian Webber")
{
    public override List<PreparationSlug> GetPreparationSlugsForTemplate(string pathToDirectoryToTemplate,
        PreparationTemplate template)
    {
        List<PreparationSlug> slugs =
        [
            new()
            {
                SlugKey = "Author",
                DisplayName = "Author",
                Type = SlugType.String,
                RequiresUserInput = true,
                SearchStrings = ["AUTHOR"]
            },
            new()
            {
                SlugKey = "Description",
                DisplayName = "Description",
                Type = SlugType.String,
                RequiresUserInput = true,
                SearchStrings = ["DESCRIPTION"]
            },
            new()
            {
                SlugKey = "Version",
                DisplayName = "Version",
                Type = SlugType.String,
                RequiresUserInput = true,
                SearchStrings = ["VERSION"]
            }
        ];

        return slugs;
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
