using ProjectTools.Core.Internal.Configuration;

namespace ProjectTools.Core.Internal.Implementations.DotSln
{
    /// <summary>
    /// Additional options for .sln Templates
    /// </summary>
    /// <seealso cref="ProjectTools.Core.Internal.Configuration.Template" />
    public class DotSlnTemplate : Template
    {
        /// <summary>
        /// The nuget settings
        /// </summary>
        public NugetSettings NugetSettings;
    }
}
