namespace ProjectTools.CL.MenuSystem;

/// <summary>
///     An attribute to register metadata for the option with the menu system.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MenuMetadata : Attribute
{
    /// <summary>
    ///     Registers metadata for the option with the menu system.
    /// </summary>
    /// <param name="priority">The priority of the option.</param>
    /// <param name="additionalArgs">Additional arguments for the command when running through the menu system.</param>
    public MenuMetadata(int priority, string additionalArgs = "")
    {
        this.Priority = priority;
        this.AdditionalArgs = additionalArgs;
    }

    /// <summary>
    ///     Additional arguments for the command when running through the menu system.
    /// </summary>
    public string AdditionalArgs { get; }

    /// <summary>
    ///     The priority of the option.
    /// </summary>
    public int Priority { get; }
}
