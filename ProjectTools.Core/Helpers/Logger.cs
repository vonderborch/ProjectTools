namespace ProjectTools.Core.Helpers;

/// <summary>
/// A class intended to help with logger messages
/// </summary>
public class Logger
{
        /// <summary>
        /// The logger method
        /// </summary>
        private readonly Func<string, bool> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public Logger(Func<string, bool> log)
        {
            _log = log;
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="indents">The indents.</param>
        /// <param name="spacesPerIndent">The spaces per indent.</param>
        /// <returns></returns>
        public void Log(string message, int indents = 0, int spacesPerIndent = 2)
        {
            var indent = new string(' ', indents * spacesPerIndent);
            LogInternal($"{indent}{message}");
        }

        /// <summary>
        /// Logs dividers.
        /// </summary>
        /// <param name="numDividers">The number of dividers.</param>
        public void LogDividers(uint numDividers)
        {
            for (var i = 0; i < numDividers; i++)
            {
                LogInternal("------------------------------------------");
            }
        }

        /// <summary>
        /// Logs new lines.
        /// </summary>
        /// <param name="numLines">The number of new lines.</param>
        public void LogNewLine(uint numLines)
        {
            for (var i = 0; i < numLines; i++)
            {
                LogInternal(" ");
            }
        }

        /// <summary>
        /// Internal log method
        /// </summary>
        /// <param name="message">The message.</param>
        private void LogInternal(string message)
        {
            _ = _log(message);
        }
}
