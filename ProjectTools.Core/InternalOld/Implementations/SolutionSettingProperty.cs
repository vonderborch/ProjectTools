namespace ProjectTools.Core.Internal.Implementations
{
    /// <summary>
    /// Information on a property of a solution setting
    /// </summary>
    public class SolutionSettingProperty
    { 
        /// <summary>
        /// The is git setting
        /// </summary>
        public bool IsGitSetting = false;

        /// <summary>
        /// The name
        /// </summary>
        public required string Name;

        /// <summary>
        /// The type
        /// </summary>
        public required Type Type;
    }
}
