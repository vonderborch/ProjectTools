namespace ProjectTools.CoreOLD.Implementations.DotSln
{
    public static class DotSlnConstants
    {
        /// <summary>
        /// The regex tags
        /// NOTE: Keep in sync with GenerateOptions().UpdateReplacementTextWithTags()
        /// </summary>
        public static readonly string[] REGEX_TAGS =
        {
            "[AUTHOR]",
            "[COMPANY]",
            "[TAGS]",
            "[DESCRIPTION]",
            "[LICENSE]",
            "[VERSION]",
        };
    }
}
