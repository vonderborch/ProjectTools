using ProjectTools.Core.PropertyHelpers;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Implementations.DotSln;

/// <summary>
/// Additional settings for .sln templates
/// </summary>
/// <seealso cref="TemplateSettings"/>
internal class DotSlnTemplateSettings : TemplateSettings
{
    /// <summary>
    /// Whether the solution using this template creates any nuget packages
    /// </summary>
    [TemplateFieldMetadata("Ask for Nuget Info", PropertyType.Bool, 100)]
    public bool AskForNugetInfo = false;

    /// <summary>
    /// The default company name for a new solution using this template
    /// </summary>
    [TemplateFieldMetadata("Default Company Name", PropertyType.String, 3)]
    public string DefaultCompanyName = string.Empty;

    /// <summary>
    /// The default nuget tags for a new solution using this template
    /// </summary>
    [TemplateFieldMetadata("Default Nuget Tags", PropertyType.String, 101, "AskForNugetInfo", true)]
    public string DefaultNugetTags = string.Empty;
}