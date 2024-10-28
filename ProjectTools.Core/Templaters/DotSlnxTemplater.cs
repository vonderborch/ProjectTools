using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

[TemplaterRegistration]
public class DotSlnTemplater()
    : AbstractBaseTemplater(".slnx", "A templater for a .slnx solutions.", "2.0.0", "Christian Webber")
{
    public override List<Slug> GetBaseSlugs(string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }
}
