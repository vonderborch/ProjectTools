using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

[TemplaterRegistration]
public class DotSlnxTemplater()
    : AbstractBaseTemplater(".sln", "A templater for a .sln solutions.", "2.0.0", "Christian Webber")
{
    public override List<Slug> GetBaseSlugs(string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }
}
