using ProjectTools.CoreOLD.Templating.Common;

namespace ProjectTools.CoreOLD.Options
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
        public required Template Template;
    }
}
