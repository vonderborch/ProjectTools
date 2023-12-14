using ProjectTools.Core.PropertyHelpers;
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
        /// The nuget tags
        /// </summary>
        [SolutionSettingFieldMetadata("Nuget Tags", nameof(DotSlnTemplateSettings.DefaultNugetTags), PropertyType.String, order: 201)]
        public required string Tags;
    }
}
