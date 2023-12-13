using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templating.Generation;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// Settings for the a .sln solution being generated
    /// </summary>
    public class DotSlnSolutionSettings : SolutionSettings
    {
        /// <summary>
        /// The company of the solution
        /// </summary>
        [SolutionSettingFieldMetadata("Company", nameof(DotSlnTemplateSettings.DefaultCompanyName), PropertyType.String, order: 3)]
        public required string Company;

        /// <summary>
        /// The nuget license expression
        /// </summary>
        [SolutionSettingFieldMetadata("License Expression", nameof(DotSlnTemplateSettings.DefaultNugetLicense), PropertyType.String, order: 200)]
        public required string LicenseExpression;

        /// <summary>
        /// The nuget tags
        /// </summary>
        [SolutionSettingFieldMetadata("Nuget Tags", nameof(DotSlnTemplateSettings.DefaultNugetTags), PropertyType.StringListComma, order: 201)]
        public required List<string> Tags;
    }
}
