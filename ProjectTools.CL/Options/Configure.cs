using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.Core;
using ProjectTools.Core.Constants;

namespace ProjectTools.CL.Options;

/// <summary>
///     An option that is used to configure the application settings.
/// </summary>
[Verb("configure", HelpText = "Configure application settings")]
public class Configure : AbstractOption
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Configure" /> class.
    /// </summary>
    public Configure()
    {
        this.AllowAutoConfiguration = false;
        this.AllowTemplateUpdates = false;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string Execute()
    {
        var settings = AppSettings.Load();
        settings ??= new AppSettings();

        settings.GitWebPath = ConsoleHelpers.GetInput("Git Web Path", settings.GitWebPath);
        settings.GitAccessToken =
            ConsoleHelpers.GetInput("Git Access Token", settings.GitAccessToken, settings.SecuredAccessToken);

        var reposText = !string.IsNullOrWhiteSpace(settings.RepositoriesListText)
            ? settings.RepositoriesListText
            : AppSettingsConstants.DefaultRepository;

        var repositories = ConsoleHelpers.GetInput("Template repositories (comma separated)", reposText);

        var newRepos = repositories.Split(',').Select(x => x.Trim()).ToList();
        foreach (var repo in newRepos)
        {
            settings.AddTemplateRepository(repo);
        }

        settings.Save();

        return "Settings Saved!";
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
    }
}
