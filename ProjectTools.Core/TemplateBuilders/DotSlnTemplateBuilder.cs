using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;

namespace ProjectTools.Core.TemplateBuilders;

public class DotSlnTemplateBuilder()
    : AbstractTemplateBuilder(".sln", "A template builder for a .sln solutions.", "2.0.0", "Christian Webber")
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

        // Add guids from the .sln file to the list of prep slugs
        var guids = GetGuids(pathToDirectoryToTemplate, pathToDirectoryToTemplate, template);
        var guidPadding = $"D{guids.Count.ToString().Length}";

        for (var i = 0; i < guids.Count; i++)
        {
            var guid = guids[i];
            var name = $"GUID{i.ToString(guidPadding)}";
            slugs.Add(new PreparationSlug
            {
                SlugKey = name,
                DisplayName = name,
                Type = SlugType.RandomGuid,
                SearchStrings = [guid],
                RequiresAnyInput = false,
                RequiresUserInput = false
            });
        }

        return slugs;
    }

    private List<string> GetGuids(string directory, string rootDirectory, PreparationTemplate template)
    {
        List<string> output = new();
        // try to find .sln files
        var files = Directory.GetFiles(directory).Where(f => Path.GetExtension(f) == ".sln");
        foreach (var file in files)
        {
            var importantLines = File.ReadLines(file)
                .Where(l => l.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase));

            foreach (var line in importantLines)
            {
                var splitByComma = line.Split(",");
                var last = splitByComma[^1];

                var guid = last[3..^2];
                output.Add(guid);
            }
        }

        // look through sub-directories
        var directories = Directory.GetDirectories(directory);
        foreach (var dir in directories)
        {
            if (!PathHelpers.PathIsInList(dir, rootDirectory, template.PrepareExcludedPaths, true, true))
            {
                var directoryGuids = GetGuids(dir, rootDirectory, template);
                output.AddRange(directoryGuids);
            }
        }

        return output.Distinct().ToList();
    }

    public override bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate)
    {
        // check if any file is a .sln file...
        foreach (var file in Directory.GetFiles(pathToDirectoryToTemplate))
        {
            if (file.EndsWith(".sln"))
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
