using CommandLine;

namespace ProjectTools.CL.Options;

/// <summary>
///     An abstract command line option
/// </summary>
public abstract class SilenceAbleAbstractOption : AbstractOption
{
    /// <summary>
    ///     Gets or sets a value indicating whether this <see cref="AbstractOption" /> is silent.
    /// </summary>
    /// <value><c>true</c> if silent; otherwise, <c>false</c>.</value>
    [Option('s', "silent", Required = false, Default = false,
        HelpText =
            "If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available.")]
    public bool Silent { get; set; }
}
