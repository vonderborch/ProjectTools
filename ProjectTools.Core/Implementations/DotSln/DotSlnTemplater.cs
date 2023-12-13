using ProjectTools.Core.Helpers;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// A Templater implementation for Visual Studio (.sln) Projects
    /// </summary>
    /// <seealso cref="ProjectTools.Core.Templating.AbstractTemplater"/>
    [ImplementationRegister("VisualStudio (.sln)", Implementation.DotSln, "Lorum Impsum")]
    internal class DotSlnTemplater : AbstractTemplater
    {
        /// <summary>
        /// Gets the type of the template information class for this implementation.
        /// </summary>
        /// <value>The type of the template information.</value>
        public override Type TemplateInformationType => typeof(TemplateInformation);

        /// <summary>
        /// Gets the type of the template settings class for this implementation.
        /// </summary>
        /// <value>The type of the template settings.</value>
        public override Type TemplateSettingsType => typeof(DotSlnTemplateSettings);

        /// <summary>
        /// Returns whether the specified directory is valid to be prepared into a template for this implementation.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>True if valid, False otherwise.</returns>
        public override bool DirectoryValidForTemplatePreperation(string directory)
        {
            // check if any file is a .sln file...
            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.EndsWith(".sln"))
                {
                    return true;
                }
            }

            foreach (var subDirectory in Directory.GetDirectories(directory))
            {
                var result = DirectoryValidForTemplatePreperation(subDirectory);
                if (result)
                {
                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Prepares the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="log">The log.</param>
        /// <returns>The result of the preperation</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string PrepareTemplate(PrepareOptions options, Func<string, bool> log)
        {
            var preparer = new DotSlnSolutionPreparer(log);
            return preparer.PrepareTemplate(options);
        }
    }
}
