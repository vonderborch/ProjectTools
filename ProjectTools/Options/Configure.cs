using CommandLine;
using ProjectTools.Core;
using ProjectTools.Helpers;

namespace ProjectTools.Options;

/// <summary>
/// A command line parameter for configuring the program.
/// </summary>
/// <seealso cref="ProjectTools.Options.AbstractOption"/>
[Verb("configure", HelpText = "Configure settings")]
internal class Configure : AbstractOption
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Configure"/> class.
    /// </summary>
    public Configure()
    {
        AllowAutoConfiguration = false;
        AllowTemplateUpdates = false;
    }

    /// <summary>
    /// Executes what this option represents.
    /// </summary>
    /// <returns>The result of the execution.</returns>
    public override string Execute()
    {
        Settings? settings = null;
        if (File.Exists(Constants.SettingsFile)) settings = Settings.LoadFile(Constants.SettingsFile);

        settings ??= new Settings();

        settings.GitWebPath = ConsoleHelpers.GetInput("Git Web Path", settings.GitWebPath);
        settings.GitAccessToken =
            ConsoleHelpers.GetInput("Git Access Token", settings.GitAccessToken, settings.SecuredAccessToken);

        var reposText = !string.IsNullOrWhiteSpace(settings.RepositoriesListText)
            ? settings.RepositoriesListText
            : Constants.DefaultTemplateRepository;

        var repositories = ConsoleHelpers.GetInput("Template repositories (comma separated)", reposText);

        settings.TemplateRepositories = repositories.Split(',').Select(x => x.Trim()).ToList();
        settings.RemoveDuplicateRepositories();

        settings.SaveFile(Constants.SettingsFile);

        return "Settings Saved!";
    }

    /// <summary>
    /// Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (Configure)option;
        Silent = options.Silent;
    }
}