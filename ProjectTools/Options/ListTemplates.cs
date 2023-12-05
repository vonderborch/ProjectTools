using System.Text;
using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.Options
{
    [Verb("list-templates", HelpText = "List all available templates")]
    internal class ListTemplates : AbstractOption
    {
        /// <summary>
        /// Gets or sets a value indicating whether [quick information].
        /// </summary>
        /// <value><c>true</c> if [quick information]; otherwise, <c>false</c>.</value>
        [Option(
            'q',
            "quick-info",
            Required = false,
            Default = false,
            HelpText = "If flag is provided, the program will just list the template names and not details on the templates."
               )]
        public bool QuickInfo { get; set; }

        /// <summary>
        /// Executes the list templates steps with the specified options.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>The result of the execution.</returns>
        public override string Execute(AbstractOption option)
        {
            var options = (ListTemplates)option;

            Console.WriteLine("Gathering available templates...");
            Manager.Instance.Templater.RefreshLocalTemplates();

            var output = new StringBuilder();
            var sortedTemplates = Manager.Instance.Templater.SortedTemplateNames;

            if (sortedTemplates.Count == 0)
            {
                Console.WriteLine("No templates found!");
                return string.Empty;
            }

            _ = Manager.Instance.Templater.Templates;
            for (var i = 0; i < sortedTemplates.Count; i++)
            {
                _ = output.AppendLine($" - {sortedTemplates[i]}");
                if (!options.QuickInfo)
                {
                    _ = output.AppendLine($"     Author: {Manager.Instance.Templater.Templates[sortedTemplates[i]].Author}");
                    _ = output.AppendLine($"     Description: {Manager.Instance.Templater.Templates[sortedTemplates[i]].Description}");
                    _ = output.AppendLine($"     Version: {Manager.Instance.Templater.Templates[sortedTemplates[i]].Version}");
                }
            }

            Console.WriteLine($"Available Templates ({sortedTemplates.Count}):");
            Console.WriteLine(output.ToString());

            return string.Empty;
        }
    }
}
