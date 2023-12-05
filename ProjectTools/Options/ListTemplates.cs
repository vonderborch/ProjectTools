using CommandLine;

namespace ProjectTools.Options
{
    [Verb("list-templates", HelpText = "List all available templates")]
    internal class ListTemplates : AbstractOption
    {
        [Option(
            'q',
            "quick-info",
            Required = false,
            Default = false,
            HelpText = "If flag is provided, the program will just list the template names and not details on the templates."
        )]
        public bool QuickInfo { get; set; }

        public override string Execute(AbstractOption option)
        {
            throw new NotImplementedException();
        }
    }
}
