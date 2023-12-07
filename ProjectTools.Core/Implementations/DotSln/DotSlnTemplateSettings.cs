using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// Additional settings for .sln templates
    /// </summary>
    /// <seealso cref="TemplateSettings"/>
    public class DotSlnTemplateSettings : TemplateSettings
    {
        /// <summary>
        /// Whether the solution using this template creates any nuget packages
        /// </summary>
        [SettingMetaAttribute("Ask for Nuget Info", SettingType.Bool)]
        public bool AskForNugetInfo = false;

        /// <summary>
        /// The default company name for a new solution using this template
        /// </summary>
        [SettingMetaAttribute("Default Company Name", SettingType.String)]
        public string DefaultCompanyName = string.Empty;

        /// <summary>
        /// The default nuget license for a new solution using this template
        /// </summary>
        [SettingMetaAttribute("Default Nuget License", SettingType.String)]
        public string DefaultNugetLicense = string.Empty;

        /// <summary>
        /// The default nuget tags for a new solution using this template
        /// </summary>
        [SettingMetaAttribute("Default Nuget Tags", SettingType.String)]
        public string DefaultNugetTags = string.Empty;
    }
}
