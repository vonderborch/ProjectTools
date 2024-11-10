using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Settings;

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
        var settings = AbstractSettings.Load();
        settings ??= new AppSettings();

        if (settings.GitSources.Count == 0)
        {
            if (ConsoleHelpers.GetYesNo("Add https://www.github.com as a git source?"))
            {
                var accessToken = ConsoleHelpers.GetInput("Git Access Token");
                settings.GitSources.Add("https://www.github.com", accessToken);
                settings.RepositoriesList.Add("https://www.github.com", TemplateConstants.DefaultTemplateRepository);
            }
        }
        else
        {
            settings.GitSources =
                ConsoleHelpers.GetStringStringDictionaryInput("Edit git sources? (git_website: access_token, ...)",
                    settings.GitSources);
        }


        settings.RepositoriesList =
            ConsoleHelpers.GetStringStringDictionaryInput(
                "Edit template repositories? (git_website: repo_link, ...)", settings.RepositoriesList);

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
