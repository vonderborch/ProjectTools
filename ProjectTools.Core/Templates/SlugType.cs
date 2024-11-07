namespace ProjectTools.Core.Templates;

/// <summary>
///     The supported slug types.
/// </summary>
public enum SlugType
{
    /// <summary>
    ///     A slug type representing a string.
    /// </summary>
    String,

    /// <summary>
    ///     A slug type representing a boolean.
    /// </summary>
    Boolean,

    /// <summary>
    ///     A slug type representing an integer.
    /// </summary>
    Int,

    /// <summary>
    ///     A slug type representing a list of strings, separated by commas.
    /// </summary>
    StringListComma,

    /// <summary>
    ///     A slug type representing a list of strings, separated by semi-colons.
    /// </summary>
    StringListSemiColan,

    /// <summary>
    ///     A slug type representing a dictionary with keys and values both being strings.
    /// </summary>
    DictionaryStringString,

    /// <summary>
    ///     A slug type representing a randomized GUID.
    /// </summary>
    RandomGuid
}
