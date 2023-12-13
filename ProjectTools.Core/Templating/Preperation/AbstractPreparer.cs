using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Templating.Preperation
{
    /// <summary>
    /// An abstract definition of a template preperer
    /// </summary>
    public abstract class AbstractPreparer
    {
        /// <summary>
        /// The logger method
        /// </summary>
        protected readonly Logger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPreparer"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public AbstractPreparer(Func<string, bool> log)
        {
            Logger = new Logger(log);
        }

        /// <summary>
        /// Prepares the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The result of the preperation</returns>
        public abstract string PrepareTemplate(PrepareOptions options);
    }
}
