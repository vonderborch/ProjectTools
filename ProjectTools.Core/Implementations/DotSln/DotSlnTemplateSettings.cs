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
        [SettingMetadata("Ask for Nuget Info", SettingType.Bool, order: 11)]
        public bool AskForNugetInfo = false;

        /// <summary>
        /// The default company name for a new solution using this template
        /// </summary>
        [SettingMetadata("Default Company Name", SettingType.String, order: 10)]
        public string DefaultCompanyName = string.Empty;

        /// <summary>
        /// The default nuget license for a new solution using this template
        /// </summary>
        [SettingMetadata("Default Nuget License", SettingType.String, order: 12)]
        public string DefaultNugetLicense = string.Empty;

        /// <summary>
        /// The default nuget tags for a new solution using this template
        /// </summary>
        [SettingMetadata("Default Nuget Tags", SettingType.String, order: 13)]
        public string DefaultNugetTags = string.Empty;
    }
}
