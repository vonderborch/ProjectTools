namespace ProjectTools.Core.Internal.Configuration
{
    /// <summary>
    /// Settings for the solution being generated
    /// </summary>
    public class SolutionSettings
    {
        /// <summary>
        /// The author of the solution
        /// </summary>
        public string Author;

        /// <summary>
        /// The company of the solution
        /// </summary>
        public string Company;

        /// <summary>
        /// The description of the solution
        /// </summary>
        public string Description;

        /// <summary>
        /// The git settings for the solution
        /// </summary>
        public SolutionGitSettings GitSettings;

        /// <summary>
        /// The name of the solution
        /// </summary>
        public string Name;

        /// <summary>
        /// The starting version of the solution
        /// </summary>
        public string Version;
    }
}
