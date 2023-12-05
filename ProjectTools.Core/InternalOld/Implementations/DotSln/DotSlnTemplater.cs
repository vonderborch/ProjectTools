namespace ProjectTools.Core.Internal.Implementations.DotSln
{
    /// <summary>
    /// A templater for .sln projects
    /// </summary>
    /// <seealso cref="Internal.Implementations.AbstractTemplater" />
    public class DotSlnTemplater : AbstractTemplater
    {
        /// <summary>
        /// The regex tags
        /// </summary>
        private static readonly string[][] _regex_tags =
        {
            new string[] { "Authors", Constants.REGEX_TAGS[0] },
            new string[] { "Company", Constants.REGEX_TAGS[1] },
            new string[] { "PackageTags", Constants.REGEX_TAGS[2] },
            new string[] { "Description", Constants.REGEX_TAGS[3] },
            new string[] { "PackageLicenseExpression", Constants.REGEX_TAGS[4] },
            new string[] { "Version", Constants.REGEX_TAGS[5] },
            new string[] { "FileVersion", Constants.REGEX_TAGS[5] },
            new string[] { "AssemblyVersion", Constants.REGEX_TAGS[5] }
        };

        /// <summary>
        /// The files to update
        /// </summary>
        private static readonly string[] _files_to_update =
        {
            ".sln",
            ".csproj",
            ".shproj",
            ".projitems"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DotSlnTemplater"/> class.
        /// </summary>
        public DotSlnTemplater()
            : base("VisualStudio (.sln)", "DotSln", TemplaterImplementations.DotSln) { }

        /// <summary>
        /// Checks whether the directory is valid for this templater.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// True if valid, False otherwise.
        /// </returns>
        public override bool DirectoryValidForTemplater(string directory)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.EndsWith(".sln"))
                {
                    return true;
                }
            }

            foreach (var dir in Directory.GetDirectories(directory))
            {
                var result = DirectoryValidForTemplater(dir);
                if (result)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Prepares the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="log"></param>
        /// <returns>
        /// The preperation result.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string Prepare(PrepareOptions options, Func<string, bool> log)
        {
            throw new NotImplementedException();
        }
    }
}
