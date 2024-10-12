using CommandLine;
using ProjectToolsOLD.CoreOLD;
using ProjectToolsOLD.CoreOLD.Options;
using ProjectToolsOLD.CoreOLD.Templating;
using ProjectToolsOLD.CoreOLD.Templating.Generation;
using ProjectToolsOLD.Helpers;

namespace ProjectToolsOLD.Options
{
    /// <summary>
    /// A command line option to generate a project from a template.
    /// </summary>
    /// <seealso cref="AbstractOption"/>
    [Verb("generate", HelpText = "Generate a project from a template")]
    internal class GenerateProject : AbstractOption
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GenerateProject"/> is force.
        /// </summary>
        /// <value><c>true</c> if force; otherwise, <c>false</c>.</value>
        [Option('f', "force", Required = false, Default = false, HelpText = "Overrides the existing directory if it already exists.")]
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Option('n', "name", Required = true, HelpText = "The name for the generated solution")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        [Option('o', "output-directory", Required = true, HelpText = "The output directory for the new solution")]
        public string OutputDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether [preserve default solution configuration].
        /// </summary>
        /// <value><c>true</c> if [preserve default solution configuration]; otherwise, <c>false</c>.</value>
        [Option('p', "preserve-default-solution-config", Required = false, Default = false, HelpText = "If flag is provided, the solution config will be preserved as the default value, if no specific config is set.")]
        public bool PreserveDefaultSolutionConfig { get; set; }

        /// <summary>
        /// Gets or sets the solution configuration.
        /// </summary>
        /// <value>The solution configuration.</value>
        [Option('c', "solution-config", Required = false, HelpText = "The specific solution config file to use.")]
        public string SolutionConfig { get; set; } = "";

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        [Option('t', "template", Required = true, HelpText = "The template to use")]
        public string Template { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether [what if].
        /// </summary>
        /// <value><c>true</c> if [what if]; otherwise, <c>false</c>.</value>
        [Option('i', "what-if", Required = false, Default = false, HelpText = "If flag is provided, the solution will not be generated, but the user will be guided through all settings.")]
        public bool WhatIf { get; set; }

        /// <summary>
        /// Executes what this option represents.
        /// </summary>
        /// <returns>The result of the execution.</returns>
        public override string Execute()
        {
            if (Silent)
            {
                throw new Exception("Silent is not a valid option for this command!");
            }

            // Validate that the template exists
            Manager.Instance.Templater.RefreshLocalTemplates();
            if (!Manager.Instance.Templater.TemplateExists(Template))
            {
                return $"Template {Template} does not exist! Please run list-templates to view all available templates.";
            }
            var template = Manager.Instance.Templater.GetTemplateByName(Template);
            _ = LogMessage($"Temple: {template.Template.Information.Name} (version {template.Template.Information.Version}) Project Generation");
            _ = LogMessage("----------------------------");

            // Get the templater for the template
            var templater = Manager.Instance.Templater.GetTemplaterForTemplate(Template);
            var settings = GetSolutionSettings(templater);

            var options = new GenerateOptions()
            {
                Force = Force,
                SolutionSettings = settings,
                OutputDirectory = OutputDirectory,
                Template = Template,
                BaseName = Name,
            };

            if (WhatIf)
            {
                return $"Solution not generated, but configuration settings saved: {SolutionConfig}";
            }
            var results = templater.GenerateProject(options, LogMessage, LogMessage, LogMessage);
            return results;
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        /// <param name="option">The option.</param>
        protected override void SetOptions(AbstractOption option)
        {
            var options = (GenerateProject)option;

            Force = options.Force;
            Name = options.Name;
            OutputDirectory = options.OutputDirectory;
            SolutionConfig = options.SolutionConfig;
            Template = options.Template;
            WhatIf = options.WhatIf;
            PreserveDefaultSolutionConfig = options.PreserveDefaultSolutionConfig;
            Silent = options.Silent;
        }

        /// <summary>
        /// Gets the solution settings.
        /// </summary>
        /// <param name="templater">The templater.</param>
        /// <returns>The user configured solution settings.</returns>
        /// <exception cref="System.Exception">Could not find a value for field {setting.Metadata.RequiredFieldName}!</exception>
        private SolutionSettings GetSolutionSettings(AbstractTemplater templater)
        {
            SolutionSettings settings;
            string backupFile;
            var solutionConfigFile = !string.IsNullOrEmpty(SolutionConfig) ? SolutionConfig : Constants.TemplatesProjectConfigurationFile;

            (var properties, var hadFile) = templater.GetGenerationSolutionSettingProperties(Template, OutputDirectory, solutionConfigFile, Name);

            if (hadFile)
            {
                _ = LogMessage($" {Environment.NewLine}SOLUTION SETTINGS");
                if (!PropertyHelpers.ContinueEditingSettings(properties, LogMessage))
                {
                    (settings, backupFile) = templater.GetSolutionSettingsForProperties(properties, solutionConfigFile, PreserveDefaultSolutionConfig);
                    _ = LogMessage($"  Configuration saved to: {backupFile}");
                    return settings;
                }
            }

            // if we didn't have an existing file, or the user wants to edit the settings, get the user input
            bool continueEditing;
            do
            {
                _ = LogMessage($" {Environment.NewLine}SOLUTION SETTINGS");

                // loop through each setting we need and ask what it should be
                foreach (var setting in properties)
                {
                    // if this setting requires another setting to be a certain value, check that value
                    if (!string.IsNullOrWhiteSpace(setting.Metadata.RequiredFieldName))
                    {
                        var otherField = properties.Where(x => x.Name == setting.Metadata.RequiredFieldName).FirstOrDefault();
                        var otherFieldValue = otherField?.CurrentValue;

                        if (otherFieldValue == null)
                        {
                            throw new Exception($"Could not find a value for field {setting.Metadata.RequiredFieldName}!");
                        }

                        var actualOtherValue = PropertyHelpers.GetDisplayValue(otherFieldValue, otherField.Type);
                        var requiredOtherValue = PropertyHelpers.GetDisplayValue(setting.Metadata.RequiredFieldValue, otherField.Type);
                        if (actualOtherValue != requiredOtherValue)
                        {
                            continue;
                        }
                    }

                    // otherwise, get the user input for the setting
                    var result = PropertyHelpers.GetInputForProperty(setting);
                    setting.CurrentValue = result;
                }

                _ = LogMessage(" ");
                continueEditing = PropertyHelpers.ContinueEditingSettings(properties, LogMessage);
            } while (continueEditing);

            (settings, backupFile) = templater.GetSolutionSettingsForProperties(properties, solutionConfigFile, PreserveDefaultSolutionConfig);
            _ = LogMessage($"  Configuration saved to: {backupFile}");
            return settings;
        }
    }
}
