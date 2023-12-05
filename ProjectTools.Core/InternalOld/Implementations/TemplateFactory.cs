using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ProjectTools.Core.Internal.Configuration;
using ProjectTools.Core.Internal.Implementations.DotSln;
using ProjectTools.Core.Internal.Repositories;
using System.Text;
using System.Text.Json;

namespace ProjectTools.Core.Internal.Implementations
{
    /// <summary>
    /// A factory to get the correct template for a given implementation
    /// </summary>
    public class TemplateFactory
    {
        /// <summary>
        /// Gets the templater for implementation.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns>The templater for the specified implementation</returns>
        /// <exception cref="System.NotImplementedException">General templater is not implemented!</exception>
        public AbstractTemplater GetTemplaterForImplementation(
            TemplaterImplementations implementation
        )
        {
            switch (implementation)
            {
                case TemplaterImplementations.DotSln:
                    return new DotSlnTemplater();

                default:
                    throw new NotImplementedException("General templater is not implemented!");
            }
        }

        /// <summary>
        /// Gets the template for contents.
        /// </summary>
        /// <param name="contents">The contents.</param>
        /// <param name="implementation">The implementation.</param>
        /// <returns>The template for the implementation</returns>
        public Template? GetTemplateForContents(
            string contents,
            TemplaterImplementations implementation
        )
        {
            switch (implementation)
            {
                case Internal.Implementations.TemplaterImplementations.DotSln:
                    return JsonSerializer.Deserialize<DotSlnTemplate>(contents);

                default:
                    throw new NotImplementedException("General templater is not implemented!");
            }
        }

        /// <summary>
        /// Gets the template for file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="repoInfo">The repo information.</param>
        /// <param name="implementation">The implementation.</param>
        /// <returns>A template for the specified file</returns>
        /// <exception cref="System.Exception">
        /// Template {template.Name} is invalid!
        /// </exception>
        public Template GetTemplateForFile(
            string file,
            TemplateGitMetadata repoInfo,
            TemplaterImplementations implementation
        )
        {
            Template? template = null;
            // Get the contents of the file
            using (var fileStream = File.OpenRead(file))
            {
                using (var zip = new ZipFile(fileStream))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        if (
                            Path.GetFileName(entry.Name) == Constants.TemplaterTemplatesInfoFileName
                        )
                        {
                            var contents = "";
                            using (var inputStream = zip.GetInputStream(entry))
                            {
                                using (var output = new MemoryStream())
                                {
                                    var buffer = new byte[4096];
                                    StreamUtils.Copy(inputStream, output, buffer);
                                    contents = Encoding.UTF8.GetString(output.ToArray());
                                }
                            }

                            // Attempt to setup the template with the file contents
                            template = GetTemplateForContents(contents, implementation);

                            if (template != null)
                            {
                                template.FilePath = file;
                                template.RepoInfo = repoInfo;
                            }
                            break;
                        }
                    }
                }
            }

            // Throw errors if bad things
            if (template == null)
            {
                throw new Exception($"{file} is not a valid template!");
            }
            if (template.Settings == null)
            {
                throw new Exception($"Template {template.Name} is invalid!");
            }

            // Setup some template settings
            var namesToCheck = new List<string>()
            {
                template.Name,
                Path.GetFileNameWithoutExtension(file)
            };
            foreach (var name in namesToCheck)
            {
                if (!template.Settings.ReplacementText.Any(x => x.Item1 == name))
                {
                    template.Settings.ReplacementText.Add(
                        new Tuple<string, string>(name, Constants.SpecialTextProjectName)
                    );
                }
            }

            return template;
        }
    }
}
