using ProjectTools.Core.Templating.Generation;

namespace ProjectTools.Core.Options
{
    /// <summary>
    /// Options for generating a project
    /// </summary>
    public class GenerateOptions
    {
        /// <summary>
        /// The base name requested
        /// </summary>
        public required string BaseName;

        /// <summary>
        /// True to force override the existing directory if it already exists.
        /// </summary>
        public required bool Force;

        /// <summary>
        /// The output directory
        /// </summary>
        public required string OutputDirectory;

        /// <summary>
        /// The solution settings
        /// </summary>
        public required SolutionSettings SolutionSettings;

        /// <summary>
        /// The template
        /// </summary>
        public required string Template;
    }
}
