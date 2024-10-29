using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

public class DotSlnTemplateBuilder()
    : AbstractTemplateBuilder(".sln", "A template builder for a .sln solutions.", "2.0.0", "Christian Webber")
{
    public override List<Slug> GetBaseSlugs(string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }
}
