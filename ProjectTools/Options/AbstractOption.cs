using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.Options
{
    internal abstract class AbstractOption
    {
        [Option(
            's',
            "silent",
            Required = false,
            Default = false,
            HelpText = "If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available."
               )]
        public bool Silent { get; set; }

        protected bool AllowAutoConfiguration { get; set; } = true;

        protected bool AllowTemplateUpdates { get; set; } = true;

        /// <summary>
        /// Executes what this option represents.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>The result of the execution.</returns>
        public abstract string Execute(AbstractOption option);

        public string ExecuteOption(AbstractOption option)
        {
            if (!Manager.Instance.ValidateSettings())
            {
                Console.WriteLine("Creating settings file...");
                _ = new Configure().Execute(new Configure());
            }
            if (AllowTemplateUpdates && Manager.Instance.Settings.ShouldUpdateTemplates)
            {
                //new UpdateTemplates().Execute(new UpdateTemplates());
            }

            return Execute(option);
        }

        protected bool LogMessage(string value)
        {
            Console.WriteLine(value);
            return true;
        }
    }
}
