using ProjectTools.Core.Internal.Configuration;

namespace ProjectTools.Core.Internal
{
    /// <summary>
    /// Options for preparing a project into a template
    /// </summary>
    public class PrepareOptions
    { 
        /// <summary>
        /// The directory of the project/solution being prepared as a template
        /// </summary>
        public required string Directory;

        /// <summary>
        /// The output directory for the final template
        /// </summary>
        public required string OutputDirectory;

        /// <summary>
        /// True to skip cleaning, False otherwise
        /// </summary>
        public required bool SkipCleaning;

        /// <summary>
        /// Settings for the template
        /// </summary>
        public required Template TemplateSettings;
    }
}
