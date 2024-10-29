using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

public class DotSlnxTemplateBuilder()
    : AbstractTemplateBuilder(".slnx", "A template builder for a .slnx solutions.", "2.0.0", "Christian Webber")
{
    public override List<Slug> GetBaseSlugs(string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }
}
