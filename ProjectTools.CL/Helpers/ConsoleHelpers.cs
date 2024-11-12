using ProjectTools.Core.Templates;

namespace ProjectTools.CL.Helpers;

/// <summary>
///     Various helper methods for the console.
/// </summary>
public static class ConsoleHelpers
{
    /// <summary>
    ///     Gets the input.
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
    ///     Gets the input.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="slugType">The slug type.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="displayValue">The display value.</param>
    /// <returns>The user's input.</returns>
    public static object? GetInput(string message, SlugType slugType, object? defaultValue, string displayValue = "")
    {
        // determine the message to the user
        var actualDefaultValue = slugType.ObjectToString(defaultValue);
        var actualDisplayValue = string.IsNullOrWhiteSpace(displayValue) ? actualDefaultValue : displayValue;

        if (!string.IsNullOrWhiteSpace(actualDisplayValue))
        {
            actualDisplayValue = $" ({actualDisplayValue})";
        }

        message = $"{message}{actualDisplayValue}: ";

        // get the user's input for the message
        Console.Write(message);
        var input = Console.ReadLine();

        var output = string.IsNullOrWhiteSpace(input) ? actualDefaultValue : input;
        var finalOutput = slugType.CorrectedValueType(output);
        return finalOutput;
    }

    public static Dictionary<string, string> GetStringStringDictionaryInput(string message,
        Dictionary<string, string> defaultValue, string keyValueSeparator = ": ", string itemSeparator = ", ")
    {
        var displayValue = StringStringDictionaryToString(defaultValue, keyValueSeparator, itemSeparator);
        while (GetYesNo($"{message}? ({displayValue})", false))
        {
            // Step 1 - Ask if user wants to delete any items from the dictionary
            while (GetYesNo($"Delete existing values? ({displayValue})", false))
            {
                var key = GetInput("Key to delete?");
                if (string.IsNullOrEmpty(key) || !defaultValue.ContainsKey(key))
                {
                    Console.WriteLine("A valid key must be specified!");
                }

                defaultValue.Remove(key);
                displayValue = StringStringDictionaryToString(defaultValue, keyValueSeparator, itemSeparator);
            }

            // Step 2 - Ask if user wants to edit any items from the dictionary
            while (GetYesNo($"Edit existing values? ({displayValue})", false))
            {
                var key = GetInput("Key to edit?");
                if (string.IsNullOrEmpty(key) || !defaultValue.ContainsKey(key))
                {
                    Console.WriteLine("A valid key must be specified!");
                }

                var newValue = GetInput("New value?", defaultValue[key]);
                defaultValue[key] = newValue;
                displayValue = StringStringDictionaryToString(defaultValue, keyValueSeparator, itemSeparator);
            }

            // Step 3 - Ask if user wants to add any items from the dictionary
            while (GetYesNo($"Add new values? ({displayValue})", false))
            {
                var key = GetInput("New Key?");
                if (string.IsNullOrEmpty(key) || defaultValue.ContainsKey(key))
                {
                    Console.WriteLine("A non-duplicate key must be specified!");
                }

                var newValue = GetInput("New value?", defaultValue[key]);
                defaultValue[key] = newValue;
                displayValue = StringStringDictionaryToString(defaultValue, keyValueSeparator, itemSeparator);
            }
        }

        return defaultValue;
    }

    private static string StringStringDictionaryToString(Dictionary<string, string> value, string keyValueSeparator,
        string itemSeparator)
    {
        List<string> partsString = new();
        foreach (var (k, v) in value)
        {
            partsString.Add($"{k}{keyValueSeparator}{v}");
        }

        var outputString = string.Join(itemSeparator, partsString);
        return outputString;
    }

    /// <summary>
    ///     Gets the input for a string list.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="separatorChar">The separator character string.</param>
    /// <param name="separatorMessage">The separator message.</param>
    /// <returns>The string list.</returns>
    public static List<string> GetStringListInput(string message, List<string> defaultValue, string separatorChar,
        string separatorMessage)
    {
        var partsString = string.Join(separatorChar, defaultValue);
        var outputString = GetInput($"{message} ({separatorMessage})", partsString);
        return outputString.Split(separatorChar, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    /// <summary>
    ///     Gets the input for a string list.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="separatorChar">The separator character string.</param>
    /// <param name="separatorMessage">The separator message.</param>
    /// <returns>The string list.</returns>
    public static List<string> GetStringListInput(string message, List<object?> defaultValue, string separatorChar,
        string separatorMessage)
    {
        List<string> parts = defaultValue.Select(x => x.ToString()).ToList();
        return GetStringListInput(message, parts, separatorChar, separatorMessage);
    }

    /// <summary>
    ///     Gets the input for an enum.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="allowedValues">The list of allowed values.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="displayValue">The display value.</param>
    /// <returns>The user's input.</returns>
    public static string GetInputWithLimit(string message, List<string> allowedValues, string defaultValue = "",
        string displayValue = "")
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
    ///     Gets whether the user agrees or not.
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

            if (key.Key == ConsoleKey.N)
            {
                return false;
            }

            if (key.Key == ConsoleKey.Enter)
            {
                return defaultYes;
            }
        }
    }

    /// <summary>
    ///     Gets the input for an enum.
    /// </summary>
    /// <param name="message">A message to display to the user.</param>
    /// <param name="defaultValue">The default value, if any.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The user's selected value.</returns>
    public static T GetEnumInput<T>(string message, T defaultValue = default) where T : Enum
    {
        var allowedValues = Enum.GetNames(typeof(T)).ToList();
        var input = GetInputWithLimit(message, allowedValues, defaultValue.ToString());
        return (T)Enum.Parse(typeof(T), input);
    }

    /// <summary>
    ///     Prints a line to the console.
    /// </summary>
    /// <param name="amount">The amount of lines to print.</param>
    public static void PrintLine(int amount = 1)
    {
        for (var i = 0; i < amount; i++)
        {
            Console.WriteLine("----------------------------------------");
        }
    }
}
