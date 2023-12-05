using ProjectTools.Core.Templating.Generation;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// Settings for the a .sln solution being generated
    /// </summary>
    public class DotSlnSolutionSettings : SolutionSettings
    {
        /// <summary>
        /// The nuget license expression
        /// </summary>
        public required string LicenseExpression;

        /// <summary>
        /// The nuget tags
        /// </summary>
        public required string[] Tags;
    }
}
