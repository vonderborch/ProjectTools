using ProjectTools.Core.Templates;

namespace ProjectTools.Core.Templaters;

public class GenericTemplateBuilder()
    : AbstractTemplateBuilder("Generic", "A template builder for a generic projects.", "2.0.0", "Christian Webber")
{
    public override List<PreparationSlug> GetPreparationSlugsForTemplate(string pathToDirectoryToTemplate,
        PreparationTemplate template)
    {
        throw new NotImplementedException();
    }

    public override bool IsValidDirectoryForBuilder(string pathToDirectoryToTemplate)
    {
        return true;
    }
}
