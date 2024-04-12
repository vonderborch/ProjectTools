using ProjectTools.Core.PropertyHelpers;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ProjectTools.Core.Templating.Common;

/// <summary>
/// Information on the template
/// </summary>
[DebuggerDisplay("{Name}")]
[JsonDerivedType(typeof(TemplateInformation), "base")]
public class TemplateInformation
{
    /// <summary>
    /// The author of the template
    /// </summary>
    [TemplateFieldMetadata("Template Author", PropertyType.String, 1)]
    public string Author = string.Empty;

    /// <summary>
    /// The description of the template
    /// </summary>
    [TemplateFieldMetadata("Template Description", PropertyType.String, 3)]
    public string Description = string.Empty;

    /// <summary>
    /// The name of the template
    /// </summary>
    [TemplateFieldMetadata("Template Name", PropertyType.String, 0)]
    public string Name = string.Empty;

    /// <summary>
    /// The version of the template
    /// </summary>
    [TemplateFieldMetadata("Template Version", PropertyType.String, 2)]
    public string Version = string.Empty;
}