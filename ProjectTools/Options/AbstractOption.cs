using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.Options
{
    internal abstract class AbstractOption
    {
        [Option('s', "silent", Required = false, Default = false, HelpText = "If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available.")]
        public bool Silent { get; set; }

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
                new Configure().Execute(new Configure());
            }
            if (Manager.Instance.Settings.ShouldUpdateTemplates)
            {
                new UpdateTemplates().Execute(new UpdateTemplates());
            }

            return Execute(option);
        }
    }
}
