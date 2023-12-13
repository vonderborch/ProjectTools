using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// Additional settings for .sln templates
    /// </summary>
    /// <seealso cref="TemplateSettings"/>
    internal class DotSlnTemplateSettings : TemplateSettings
    {
        /// <summary>
        /// Whether the solution using this template creates any nuget packages
        /// </summary>
        [TemplateFieldMetadata("Ask for Nuget Info", PropertyType.Bool, order: 100)]
        public bool AskForNugetInfo = false;

        /// <summary>
        /// The default company name for a new solution using this template
        /// </summary>
        [TemplateFieldMetadata("Default Company Name", PropertyType.String, order: 3)]
        public string DefaultCompanyName = string.Empty;

        /// <summary>
        /// The default nuget license for a new solution using this template
        /// </summary>
        [TemplateFieldMetadata("Default Nuget License", PropertyType.String, order: 101, requiredFieldName: "AskForNugetInfo", requiredFieldValue: true)]
        public string DefaultNugetLicense = string.Empty;

        /// <summary>
        /// The default nuget tags for a new solution using this template
        /// </summary>
        [TemplateFieldMetadata("Default Nuget Tags", PropertyType.String, order: 102, requiredFieldName: "AskForNugetInfo", requiredFieldValue: true)]
        public string DefaultNugetTags = string.Empty;
    }
}
