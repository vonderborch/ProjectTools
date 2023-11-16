namespace ProjectTools.Core.Internal.Implementations.DotSln
{
    /// <summary>
    /// A templater for .sln projects
    /// </summary>
    /// <seealso cref="Implementations.AbstractTemplater" />
    public class DotSlnTemplater : AbstractTemplater
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotSlnTemplater"/> class.
        /// </summary>
        public DotSlnTemplater() : base("Visual Studio Solution Templater", "DotSln", TemplaterImplementations.DotSln)
        {
        }

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
