using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Templating.Generation
{
    /// <summary>
    /// An abstract project generator.
    /// </summary>
    internal abstract class AbstractGenerator
    {
        /// <summary>
        /// The logger method
        /// </summary>
        protected readonly Logger CommandLogger;

        /// <summary>
        /// The logger method
        /// </summary>
        protected readonly Logger InstructionLogger;

        /// <summary>
        /// The logger method
        /// </summary>
        protected readonly Logger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGenerator"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public AbstractGenerator(Func<string, bool> log, Func<string, bool> instructionLog, Func<string, bool> commandLog)
        {
            Logger = new Logger(log);
            InstructionLogger = new Logger(instructionLog);
            CommandLogger = new Logger(commandLog);
        }

        /// <summary>
        /// Generates the project.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The result of the generation</returns>
        public abstract string GenerateProject(GenerateOptions options);
    }
}
