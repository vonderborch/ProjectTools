using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ProjectTools.Core.Internal.Repositories;
using ProjectTools.Core.Internal.Templaters.DotSln;
using System.Text;
using System.Text.Json;

namespace ProjectTools.Core.Internal.Configuration
{
    public static class TemplateFactory
    {
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

                            switch (implementation)
                            {
                                case TemplaterImplementations.DotSln:
                                    template = JsonSerializer.Deserialize<DotSlnTemplate>(contents);
                                    break;
                                default:
                                    template = JsonSerializer.Deserialize<Template>(contents);
                                    break;
                            }
                            
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
