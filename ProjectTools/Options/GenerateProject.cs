using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.Options
{
    /// <summary>
    /// A command line option to generate a project from a template.
    /// </summary>
    /// <seealso cref="ProjectTools.Options.AbstractOption"/>
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
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        [Option('o', "output-directory", Required = true, HelpText = "The output directory for the new solution")]
        public required string OutputDirectory { get; set; }

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
        public required string Template { get; set; }

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

            // Get the templater for the template
            var templater = Manager.Instance.Templater.GetTemplaterForTemplate(Template);

            throw new NotImplementedException();
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
            Silent = options.Silent;
        }
    }
}
