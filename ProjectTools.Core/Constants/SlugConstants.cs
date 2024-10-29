using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Constants;

/// <summary>
/// Constants related to slugs.
/// </summary>
public static class SlugConstants
{
    /// <summary>
    /// Gets a list of base slugs to parameterize the template with.
    /// </summary>
    /// <returns>The list of base preparation slugs</returns>
    public static List<PreperationSlug> GetBasePreparationSlugs()
    {
        return
        [
            new PreperationSlug
            {
                SlugKey = "SolutionName",
                DisplayName = "Solution Name",
                Type = SlugType.String,
                RequiresUserInput = true,
                SearchStrings = ["SolutionName"]
            }
        ];
    }
}
