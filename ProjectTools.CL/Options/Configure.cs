using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Settings;

namespace ProjectTools.CL.Options;

/// <summary>
///     An option that is used to configure the application settings.
/// </summary>
[Verb("configure", HelpText = "Configure application settings")]
[MenuMetadata(11)]
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
        var settings = AbstractSettings.Load();
        settings ??= new AppSettings();

        if (settings.GitSourcesAndAccessTokens.Count == 0)
        {
            if (ConsoleHelpers.GetYesNo("Add https://www.github.com as a git source?"))
            {
                var accessToken = ConsoleHelpers.GetInput("Git Access Token");
                settings.GitSourcesAndAccessTokens.Add("https://www.github.com", accessToken);
                settings.RepositoriesAndGitSources.Add("https://www.github.com",
                    TemplateConstants.DefaultTemplateRepository);
            }
        }
        else
        {
            settings.GitSourcesAndAccessTokens =
                ConsoleHelpers.GetStringStringDictionaryInput("Edit git sources? (git_website: access_token, ...)",
                    settings.GitSourcesAndAccessTokens);
        }


        settings.RepositoriesAndGitSources =
            ConsoleHelpers.GetStringStringDictionaryInput(
                "Edit template repositories? (repo_link: git_website, ...)", settings.RepositoriesAndGitSources);

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
