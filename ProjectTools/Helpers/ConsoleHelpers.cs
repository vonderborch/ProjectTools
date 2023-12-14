namespace ProjectTools.Helpers
{
    public static class ConsoleHelpers
    {
        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="displayValue">The display value.</param>
        /// <returns>The user's input.</returns>
        public static string GetInput(string message, string defaultValue = "", string displayValue = "")
        {
            // determine the message to the user
            var actualDisplayValue = string.IsNullOrWhiteSpace(displayValue) ? defaultValue : displayValue;
            if (!string.IsNullOrWhiteSpace(actualDisplayValue))
            {
                actualDisplayValue = $" ({actualDisplayValue})";
            }
            message = $"{message}{actualDisplayValue}: ";

            // get the user's input for the message
            Console.Write(message);
            var input = Console.ReadLine();

            var output = string.IsNullOrWhiteSpace(input) ? defaultValue : input;
            return output;
        }

        /// <summary>
        /// Gets the input for an enum.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="allowedValues">The list of allowed values.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="displayValue">The display value.</param>
        /// <returns>The user's input.</returns>
        public static string GetInputWithLimit(string message, List<string> allowedValues, string defaultValue = "", string displayValue = "")
        {
            // determine the message to the user
            var actualDisplayValue = string.IsNullOrWhiteSpace(displayValue) ? defaultValue : displayValue;
            if (!string.IsNullOrWhiteSpace(actualDisplayValue))
            {
                actualDisplayValue = $" ({actualDisplayValue})";
            }
            var allowedValuesDisplay = string.Join(", ", allowedValues);
            message = $"{message} (Allowed Values: {allowedValuesDisplay}){actualDisplayValue}: ";

            // get the user's input for the message
            string output;
            do
            {
                Console.Write(message);
                var input = Console.ReadLine();

                output = string.IsNullOrWhiteSpace(input) ? defaultValue : input;
            } while (!allowedValues.Contains(output));

            return output;
        }

        /// <summary>
        /// Gets whether the user agrees or not.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="defaultYes">if set to <c>true</c> [default yes].</param>
        /// <returns>True if Yes, False if No.</returns>
        public static bool GetYesNo(string message, bool defaultYes = true)
        {
            while (true)
            {
                Console.Write($"{message} ({(defaultYes ? "Y/n" : "y/N")}) ");
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (key.Key == ConsoleKey.N)
                {
                    return false;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    return defaultYes;
                }
            }
        }
    }
}
