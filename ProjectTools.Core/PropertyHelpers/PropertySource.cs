namespace ProjectTools.Core.PropertyHelpers
{
    /// <summary>
    /// Sources available for Template Settings
    /// </summary>
    public enum PropertySource
    {
        /// <summary>
        /// The setting comes from a template information class
        /// </summary>
        TemplateInformation,

        /// <summary>
        /// The setting comes from a template settings class
        /// </summary>
        TemplateSettings,

        /// <summary>
        /// The setting comes from a solution settings class
        /// </summary>
        SolutionSettings,
    }
}
