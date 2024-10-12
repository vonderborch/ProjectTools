namespace ProjectTools.CoreOLD.PropertyHelpers
{
    /// <summary>
    /// Types available for Template Settings
    /// </summary>
    public enum PropertyType
    {
        /// <summary>
        /// Setting is a boolean
        /// </summary>
        Bool,

        /// <summary>
        /// Setting is a string
        /// </summary>
        String,

        /// <summary> Setting is a List<string> separated by commas </summary>
        StringListComma,

        /// <summary> Setting is a List<string> separated by semi-colans </summary>
        StringListSemiColan,

        /// <summary> Setting is a Dictionary<string, string> </summary>
        DictionaryStringString,

        /// <summary>
        /// Setting is an enum
        /// </summary>
        Enum,
    }
}
