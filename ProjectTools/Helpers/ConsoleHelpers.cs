namespace ProjectTools.Helpers
{
    public static class ConsoleHelpers
    {
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
    }
}
