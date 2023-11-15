using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ProjectTools.Core.Internal.Configuration;
using ProjectTools.Core.Internal.Implementations.DotSln;
using ProjectTools.Core.Internal.Repositories;
using System.Text;
using System.Text.Json;

namespace ProjectTools.Core.Internal.Implementations
{
    public static class TemplateFactory
    {
        public static List<AbstractTemplater> TemplaterImplementations => new()
            {
                new DotSlnTemplater(),
            };

        public static Template? GetTemplateForContents(string contents, TemplaterImplementations implementation)
        {
            switch (implementation)
            {
                case TemplaterImplementations.DotSln:
                    return JsonSerializer.Deserialize<DotSlnTemplate>(contents);
                default:
                    return JsonSerializer.Deserialize<Template>(contents);
            }
        }

        public static Template GetTemplateForFile(string file, TemplateGitInfo repoInfo, TemplaterImplementations implementation)
        {
            Template? template = null;
            using (var fileStream = File.OpenRead(file))
            {
                using (var zip = new ZipFile(fileStream))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        if (Path.GetFileName(entry.Name) == Constants.TemplaterTemplatesInfoFileName)
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

            if (template == null)
            {
                throw new Exception($"{file} is not a valid template!");
            }
            if (template.Settings == null)
            {
                throw new Exception($"Template {template.Name} is invalid!");
            }

            var namesToCheck = new List<string>() { template.Name, Path.GetFileNameWithoutExtension(file) };
            foreach (var name in namesToCheck)
            {
                if (!template.Settings.ReplacementText.Any(x => x.Item1 == name))
                {
                    template.Settings.ReplacementText.Add(new Tuple<string, string>(name, Constants.SpecialTextProjectName));
                }
            }

            return template;
        }
    }
}
