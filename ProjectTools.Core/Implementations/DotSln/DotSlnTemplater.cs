using ProjectTools.Core.Helpers;
using ProjectTools.Core.Options;
using ProjectTools.Core.PropertyHelpers;
using ProjectTools.Core.Templating;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// A Templater implementation for Visual Studio (.sln) Projects
    /// </summary>
    /// <seealso cref="ProjectTools.Core.Templating.AbstractTemplater"/>
    [ImplementationRegister("VisualStudio (.sln)", Implementation.DotSln, "Lorum Impsum")]
    internal class DotSlnTemplater : AbstractTemplater
    {
        /// <summary>
        /// Gets the type of the solution settings.
        /// </summary>
        /// <value>The type of the solution settings.</value>
        public override Type SolutionSettingsType => typeof(DotSlnSolutionSettings);

        /// <summary>
        /// Gets the type of the template information class for this implementation.
        /// </summary>
        /// <value>The type of the template information.</value>
        public override Type TemplateInformationType => typeof(TemplateInformation);

        /// <summary>
        /// Gets the type of the template settings class for this implementation.
        /// </summary>
        /// <value>The type of the template settings.</value>
        public override Type TemplateSettingsType => typeof(DotSlnTemplateSettings);

        /// <summary>
        /// Returns whether the specified directory is valid to be prepared into a template for this implementation.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>True if valid, False otherwise.</returns>
        public override bool DirectoryValidForTemplatePreperation(string directory)
        {
            // check if any file is a .sln file...
            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.EndsWith(".sln"))
                {
                    return true;
                }
            }

            foreach (var subDirectory in Directory.GetDirectories(directory))
            {
                var result = DirectoryValidForTemplatePreperation(subDirectory);
                if (result)
                {
                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Generates the project.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="log">The log.</param>
        /// <param name="instructionLog"></param>
        /// <param name="commandLog"></param>
        /// <returns>The result of the generation</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string GenerateProject(GenerateOptions options, Func<string, bool> log, Func<string, bool> instructionLog, Func<string, bool> commandLog)
        {
            var generator = new DotSlnGenerator(log, instructionLog, commandLog);
            return generator.GenerateProject(options);
        }

        /// <summary>
        /// Prepares the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="log">The log.</param>
        /// <returns>The result of the preperation</returns>
        public override string PrepareTemplate(PrepareOptions options, Func<string, bool> log)
        {
            var preparer = new DotSlnSolutionPreparer(log);
            return preparer.PrepareTemplate(options);
        }

        /// <summary>
        /// Modifies the solution setting properties.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="outputDir">The output directory.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="solutionName">The solution name.</param>
        /// <returns>The corrected solution setting properties.</returns>
        protected override List<Property> ModifySolutionSettingProperties(string templateName, string outputDir, List<Property> properties, string solutionName)
        {
            var template = Manager.Instance.Templater.GetTemplateByName(templateName);
            var templateInformation = template.Template.Information;
            var templateSettings = (DotSlnTemplateSettings)template.Template.Settings;

            var output = new List<Property>();
            var nugetFields = new List<string>() { nameof(DotSlnSolutionSettings.LicenseExpression), nameof(DotSlnSolutionSettings.Tags) };
            foreach (var property in properties)
            {
                // skip the license expression and tags if we don't ask for nuget info
                if (nugetFields.Contains(property.Name) && !templateSettings.AskForNugetInfo)
                {
                    continue;
                }

                // If we don't have a default value, let's populate one from settings...
                if (property.CurrentValue == null)
                {
                    var metadata = (SolutionSettingFieldMetadata)property.Metadata;
                    var defaultField = metadata.TemplateSettingFieldName;
                    var defaultFromTemplateSettings = templateSettings.GetType()?.GetField(defaultField)?.GetValue(templateSettings);

                    if (metadata.Type != PropertyType.String)
                    {
                        property.CurrentValue = defaultFromTemplateSettings;
                    }
                    else
                    {
                        var actualDefault = (string)defaultFromTemplateSettings;
                        actualDefault = UpdateSimpleReplacementTextDuringSettingsRequest(actualDefault, outputDir, solutionName);
                        property.CurrentValue = actualDefault;
                    }
                }

                output.Add(property);
            }

            return output;
        }
    }
}
