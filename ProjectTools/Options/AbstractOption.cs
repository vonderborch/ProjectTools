using CommandLine;
using ProjectTools.Core;

namespace ProjectTools.Options
{
    /// <summary>
    /// An abstract command line option
    /// </summary>
    internal abstract class AbstractOption
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AbstractOption"/> is silent.
        /// </summary>
        /// <value><c>true</c> if silent; otherwise, <c>false</c>.</value>
        [Option('s', "silent", Required = false, Default = false, HelpText = "If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available.")]
        public bool Silent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow automatic configuration].
        /// </summary>
        /// <value><c>true</c> if [allow automatic configuration]; otherwise, <c>false</c>.</value>
        protected bool AllowAutoConfiguration { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow template updates].
        /// </summary>
        /// <value><c>true</c> if [allow template updates]; otherwise, <c>false</c>.</value>
        protected bool AllowTemplateUpdates { get; set; } = true;

        /// <summary>
        /// Executes what this option represents.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>The result of the execution.</returns>
        public abstract string Execute();

        /// <summary>
        /// Executes the option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>The result.</returns>
        public string ExecuteOption(AbstractOption option)
        {
            if (!Manager.Instance.ValidateSettings())
            {
                _ = LogMessage("Creating settings file...");
                var configure = new Configure() { Silent = Silent };
                _ = configure.Execute();
            }
            if (AllowTemplateUpdates && Manager.Instance.Settings.ShouldUpdateTemplates)
            {
                var updateTemplates = new UpdateTemplates() { Silent = Silent };
                if (Silent)
                {
                    updateTemplates.ForceUpdate = true;
                }
                _ = updateTemplates.Execute();
            }

            SetOptions(option);
            return Execute();
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True to indicate a message was wrwitten.</returns>
        protected bool LogMessage(string value)
        {
            Console.WriteLine(value);
            return true;
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        /// <param name="option">The option.</param>
        protected abstract void SetOptions(AbstractOption option);
    }
}
