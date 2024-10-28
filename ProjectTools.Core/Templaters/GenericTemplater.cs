using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

[TemplaterRegistration]
public class GenericTemplater()
    : AbstractBaseTemplater("Generic", "A templater for a generic projects.", "2.0.0", "Christian Webber")
{
    public override List<Slug> GetBaseSlugs(string pathToDirectoryToTemplate)
    {
        throw new NotImplementedException();
    }
}
