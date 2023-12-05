using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Options
{
    public class PrepareOptions
    {
        /// <summary>
        /// The directory
        /// </summary>
        public required string Directory;

        /// <summary>
        /// The output directory
        /// </summary>
        public required string OutputDirectory;

        /// <summary>
        /// The skip cleaning
        /// </summary>
        public required bool SkipCleaning;

        /// <summary>
        /// The template settings
        /// </summary>
        public required AbstractTemplate TemplateSettings;
    }
}
