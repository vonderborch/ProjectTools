#region

using ProjectTools.Core.Templates;

#endregion

namespace ProjectTools.Core.Helpers;

/// <summary>
///     Various helper methods for slugs.
/// </summary>
public static class SlugHelpers
{
    /// <summary>
    ///     Updates the slug's current values with those specified in a project config file.
    /// </summary>
    /// <param name="projectConfigFile">The project config file.</param>
    /// <param name="slugs">The slugs.</param>
    /// <returns>The updated slugs.</returns>
    public static List<Slug> GetSlugsFromProjectConfigFile(string projectConfigFile, List<Slug> slugs)
    {
        if (!File.Exists(projectConfigFile))
        {
            return slugs;
        }

        var slugValues = JsonHelpers.DeserializeFromFile<Dictionary<string, string>>(projectConfigFile);
        if (slugValues == null)
        {
            return slugs;
        }

        foreach (var slug in slugs)
        {
            if (slugValues.TryGetValue(slug.SlugKey, out var value))
            {
                slug.CurrentValue = value;
            }
        }

        return slugs;
    }

    /// <summary>
    ///     Gets values for the slugs in a dictionary format.
    /// </summary>
    /// <param name="slugs">The slugs.</param>
    /// <param name="useSlugKeyOrDisplayName">True to use Slug.SlugKey, False to use Slug.DisplayName.</param>
    /// <returns>A dictionary representing the slugs.</returns>
    public static Dictionary<string, string> GetSlugValues(List<Slug> slugs, bool useSlugKeyOrDisplayName)
    {
        if (useSlugKeyOrDisplayName)
        {
            return slugs.ToDictionary(x => x.SlugKey, x => x.CurrentValue);
        }

        return slugs.ToDictionary(x => x.DisplayName, x => x.CurrentValue);
    }

    /// <summary>
    ///     Saves the values for the slugs to the specified file.
    /// </summary>
    /// <param name="projectConfigFile">The file.</param>
    /// <param name="slugs">The slugs.</param>
    public static void SaveSlugsToProjectConfigFile(string projectConfigFile, List<Slug> slugs)
    {
        var slugValues = GetSlugValues(slugs, true);
        JsonHelpers.SerializeToFile(projectConfigFile, slugValues);
    }
}
