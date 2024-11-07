using System.Reflection;
using CommandLine;
using ProjectTools.CL.Options;

namespace ProjectTools.CL;

/// <summary>
///     A helper class for command line arguments.
/// </summary>
public class CommandHelper
{
    /// <summary>
    ///     Stores the available command line option classes.
    /// </summary>
    private List<Type> _avaliableOptions;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandHelper" /> class.
    /// </summary>
    public CommandHelper()
    {
    }

    public List<Type> GetTypes()
    {
        if (this._avaliableOptions == null)
        {
            var assembly = Assembly.GetEntryAssembly();
            var types = assembly.GetTypes();

            this._avaliableOptions = types.Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToList();
        }

        return this._avaliableOptions;
    }

    /// <summary>
    ///     Finds the available root commands.
    /// </summary>
    /// <returns>The available root commands.</returns>
    public List<string> GetAvailableRootCommands()
    {
        var rootCommands = new List<string>();

        foreach (var option in GetTypes())
        {
            var verbAttribute = (VerbAttribute)option.GetCustomAttribute(typeof(VerbAttribute), false);
            if (verbAttribute != null)
            {
                rootCommands.Add(verbAttribute.Name);
            }
        }

        return rootCommands;
    }

    /// <summary>
    ///     Parses the provided arguments and executes the appropriate command.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public void ParseAndExecuteArguments(string[] args)
    {
        Parser.Default.ParseArguments(args, GetTypes().ToArray())
            .WithParsed(Run)
            .WithNotParsed(HandleErrors);
    }

    /// <summary>
    ///     Handles any errors.
    /// </summary>
    private static void HandleErrors(IEnumerable<Error> obj)
    {
    }

    private void Run(object obj)
    {
        var options = (AbstractOption)obj;
        var result = options.ExecuteOption(options);

        // print the result if there is one
        if (!string.IsNullOrWhiteSpace(result))
        {
            Console.WriteLine(result);
        }
    }
}
