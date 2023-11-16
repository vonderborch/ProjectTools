using ProjectTools.Core.Internal.Configuration;
using ProjectTools.Core.Internal.Implementations.DotSln;
using System.Reflection;

namespace ProjectTools.Core.Internal.Implementations
{
    /// <summary>
    /// A factory to get the correct settings for a given implementation
    /// </summary>
    public class SolutionSettingsFactory
    {
        /// <summary>
        /// Gets the fields and types for implementation.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns>A list of fields and types for the solution settings of an implementation</returns>
        public List<SolutionSettingProperty> GetFieldsAndTypesForImplementation(TemplaterImplementations implementation)
        {
            // Get field info for the specific implementation
            FieldInfo[] fields;
            switch (implementation)
            {
                case TemplaterImplementations.DotSln:
                    fields = typeof(DotSlnSolutionSettings).GetFields();
                    break;

                default:
                    fields = typeof(SolutionSettings).GetFields();
                    break;
            }
            var output = new List<SolutionSettingProperty>();
            foreach (var field in fields)
            {
                // Skip the git settings
                if (field.Name == "GitSettings")
                {
                    continue;
                }
                output.Add(new SolutionSettingProperty()
                {
                    Name = field.Name,
                    Type = field.FieldType,
                });
            }

            // Get field info for the git settings
            var gitFields = typeof(SolutionGitSettings).GetFields();
            foreach (var field in gitFields)
            {
                output.Add(new SolutionSettingProperty()
                {
                    Name = field.Name,
                    Type = field.FieldType,
                    IsGitSetting = true,
                });
            }

            return output;
        }

        /// <summary>
        /// Gets the settings for implementation.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns>A solution settings object for a given implementation</returns>
        public SolutionSettings GetSettingsForImplementation(TemplaterImplementations implementation)
        {
            switch (implementation)
            {
                case TemplaterImplementations.DotSln:
                    return new DotSlnSolutionSettings();

                default:
                    return new SolutionSettings();
            }
        }
    }
}
