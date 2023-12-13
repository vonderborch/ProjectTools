using System.Text;
using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.Options
{
    /// <summary>
    /// A command to list available templates
    /// </summary>
    /// <seealso cref="ProjectTools.Options.AbstractOption"/>
    [Verb("list-templates", HelpText = "List all available templates")]
    internal class ListTemplates : AbstractOption
    {
        /// <summary>
        /// Gets or sets a value indicating whether [quick information].
        /// </summary>
        /// <value><c>true</c> if [quick information]; otherwise, <c>false</c>.</value>
        [Option('q', "quick-info", Required = false, Default = false, HelpText = "If flag is provided, the program will just list the template names and not details on the templates.")]
        public bool QuickInfo { get; set; }

        /// <summary>
        /// Executes the list templates steps with the specified options.
        /// </summary>
        /// <returns>The result of the execution.</returns>
        public override string Execute()
        {
            _ = LogMessage("Gathering available templates...");
            Manager.Instance.Templater.RefreshLocalTemplates();

            var output = new StringBuilder();
            Manager.Instance.Templater.RefreshLocalTemplates();
            var sortedTemplates = Manager.Instance.Templater.SortedLocalTemplateNames;

            if (sortedTemplates.Count == 0)
            {
                _ = LogMessage("No templates found!");
                return string.Empty;
            }

            // list each template
            foreach (var name in sortedTemplates)
            {
                var template = Manager.Instance.Templater.GetTemplateByName(name);
                _ = output.AppendLine($" - {template.Template.Information.Name}");
                if (!QuickInfo)
                {
                    _ = output.AppendLine($"     Author: {template.Template.Information.Author}");
                    _ = output.AppendLine($"     Description: {template.Template.Information.Description}");
                    _ = output.AppendLine($"     Version: {template.Template.Information.Version}");
                }
            }

            _ = LogMessage($"Available Templates ({sortedTemplates.Count}):");
            _ = LogMessage(output.ToString());

            return string.Empty;
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        /// <param name="option">The option.</param>
        protected override void SetOptions(AbstractOption option)
        {
            var options = (ListTemplates)option;
            QuickInfo = options.QuickInfo;
            Silent = options.Silent;
        }
    }
}
