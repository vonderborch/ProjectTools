using CommandLine;

namespace ProjectTools.CL.Options;

[Verb("generate", HelpText = "Generate a project from a template")]
public class GenerateProject : AbstractOption
{
    public override string Execute()
    {
        throw new NotImplementedException();
    }

    protected override void SetOptions(AbstractOption option)
    {
        throw new NotImplementedException();
    }
}
