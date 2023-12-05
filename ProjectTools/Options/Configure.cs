using CommandLine;
using ProjectTools.Core;
using ProjectTools.Helpers;

namespace ProjectTools.Options
{
    [Verb("configure", HelpText = "Configure settings")]
    internal class Configure : AbstractOption
    {
        public Configure()
        {
            AllowAutoConfiguration = false;
            AllowTemplateUpdates = false;
        }

        public override string Execute(AbstractOption option)
        {
            Settings? settings = null;
            if (File.Exists(Constants.SettingsFile))
            {
                settings = Settings.LoadFile(Constants.SettingsFile);
            }

            settings ??= new Settings();

            settings.GitWebPath = ConsoleHelpers.GetInput("Git Web Path", settings.GitWebPath);
            settings.GitAccessToken = ConsoleHelpers.GetInput(
                "Git Access Token",
                settings.GitAccessToken,
                settings.SecuredAccessToken
                                                             );

            var reposText = !string.IsNullOrWhiteSpace(settings.RepositoriesListText) ? settings.RepositoriesListText : Constants.DefaultTemplateRepository;

            var repositories = ConsoleHelpers.GetInput("Template repositories (comma separated)", reposText);

            settings.TemplateRepositories = repositories.Split(',').Select(x => x.Trim()).ToList();
            settings.RemoveDuplicateRepositories();

            settings.SaveFile(Constants.SettingsFile);

            return "Settings Saved!";
        }
    }
}
