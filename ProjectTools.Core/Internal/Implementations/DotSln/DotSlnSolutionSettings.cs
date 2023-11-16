using ProjectTools.Core.Internal.Configuration;

namespace ProjectTools.Core.Internal.Implementations.DotSln
{
    /// <summary>
    /// Specific additional settings for .sln Templates
    /// </summary>
    /// <seealso cref="ProjectTools.Core.Internal.Configuration.SolutionSettings" />
    public class DotSlnSolutionSettings : SolutionSettings
    {
        /// <summary>
        /// The nuget license expression
        /// </summary>
        public string LicenseExpression;

        /// <summary>
        /// The nuget tags
        /// </summary>
        public string[] Tags;
    }
}
