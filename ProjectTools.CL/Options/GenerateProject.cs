using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templates;

namespace ProjectTools.CL.Options;

/// <summary>
///     A command line option to generate a new project from a template.
/// </summary>
[Verb("generate", HelpText = "Generate a new project from a template")]
[MenuMetadata(99)]
public class GenerateProject : AbstractOption
{
    /// <summary>
    ///     The special value handler.
    /// </summary>
    private SpecialValueHandler? _specialValueHandler;

    /// <summary>
    ///     Gets or sets a value indicating whether to override the existing directory if it already exists.
    /// </summary>
    [Option('f', "force", Required = false, Default = false,
        HelpText = "Overrides the existing directory if it already exists.")]
    public bool Force { get; set; }

    /// <summary>
    ///     Gets or sets the parent output directory.
    /// </summary>
    [Option('o', "parent-output-directory", Required = true,
        HelpText = "The parent output directory for the new solution")]
    public string ParentOutputDirectory { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the template to use.
    /// </summary>
    [Option('t', "template", Required = true, HelpText = "The template to use")]
    public string Template { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the new project.
    /// </summary>
    [Option('n', "name", Required = true, HelpText = "The name of the project")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the generate project configuration file to use.
    /// </summary>
    [Option('c', "generate-project-config", Required = false,
        HelpText = "The specific generate project configuration file to use.")]
    public string GenerateProjectConfiguration { get; set; } = "";

    /// <summary>
    ///     Gets or sets whether we should actually do any work or not.
    /// </summary>
    [Option('i', "what-if", Required = false, Default = false,
        HelpText =
            "If flag is provided, the solution will not be generated, but the user will be guided through all settings.")]
    public bool WhatIf { get; set; }

    [Option('r', "remove-existing-config", Required = false, Default = false,
        HelpText =
            "If flag is provided, we will delete the existing project generation config file before execute anything.")]
    public bool RemoveExistingProjectGenerationConfigurationFile { get; set; }

    /// <summary>
    ///     Executes what this option represents.
    /// </summary>
    /// <returns>The result of the execution.</returns>
    public override string Execute()
    {
        // Step 1 - Load local templates and find the one we want to use...
        var localTemplates = new LocalTemplates();
        var availableTemplates = localTemplates.Templates;
        if (availableTemplates.Count == 0)
        {
            return "No templates found!";
        }

        var templateNames = localTemplates.TemplateNames;
        if (!templateNames.Contains(this.Template))
        {
            return $"Template '{this.Template}' not found, please run list-templates to see available templates.";
        }

        var templateToUse = availableTemplates.First(t => t.Name == this.Template);
        LogMessage(
            $"Using Template {templateToUse.Name} (Version {templateToUse.Template.Version}) to Generate Project");
        LogMessage("--------------------------------------");

        // Step 2 - Check if we're using a specific generate project configuration file or create a new one...
        IOHelpers.CreateDirectoryIfNotExists(PathConstants.ProjectGenerationConfigDirectory);
        this.GenerateProjectConfiguration = string.IsNullOrWhiteSpace(this.GenerateProjectConfiguration)
            ? PathConstants.DefaultProjectGenerationConfigFileName
            : this.GenerateProjectConfiguration;
        var projectConfigFile =
            Path.Combine(PathConstants.ProjectGenerationConfigDirectory, this.GenerateProjectConfiguration);

        if (this.RemoveExistingProjectGenerationConfigurationFile && File.Exists(projectConfigFile))
        {
            File.Delete(projectConfigFile);
        }

        // Step 3 - Get User Configuration Settings
        this._specialValueHandler =
            new SpecialValueHandler(this.ParentOutputDirectory, this.Name, templateToUse.Template);
        var actualTemplate = UpdateTemplateWithUserInput(projectConfigFile, templateToUse.Template);

        // Step 4 - Generate the project!
        Logger logger = new(LogMessage);
        return actualTemplate.GenerateProject(this.ParentOutputDirectory, this.Name, templateToUse.LocalPath, logger,
            logger, logger, this.Force);
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (GenerateProject)option;

        this.Force = options.Force;
        this.ParentOutputDirectory = options.ParentOutputDirectory;
        this.Template = options.Template;
        this.WhatIf = options.WhatIf;
        this.GenerateProjectConfiguration = options.GenerateProjectConfiguration;
        this.RemoveExistingProjectGenerationConfigurationFile =
            options.RemoveExistingProjectGenerationConfigurationFile;
    }

    /// <summary>
    ///     Updates the slug's current values with those specified in a project config file.
    /// </summary>
    /// <param name="projectConfigFile">The project config file.</param>
    /// <param name="slugs">The slugs.</param>
    /// <returns>The updated slugs.</returns>
    private List<Slug> GetSlugsFromProjectConfigFile(string projectConfigFile, List<Slug> slugs)
    {
        if (!File.Exists(projectConfigFile))
        {
            return slugs;
        }

        var slugValues = JsonHelpers.DeserializeFromFile<Dictionary<string, object?>>(projectConfigFile);
        if (slugValues == null)
        {
            return slugs;
        }

        foreach (var slug in slugs)
        {
            if (slugValues.TryGetValue(slug.SlugKey, out var value))
            {
                slug.CurrentValue = value;
            }
        }

        return slugs;
    }

    /// <summary>
    ///     Saves the values for the slugs to the specified file.
    /// </summary>
    /// <param name="projectConfigFile">The file.</param>
    /// <param name="slugs">The slugs.</param>
    private void SaveSlugsToProjectConfigFile(string projectConfigFile, List<Slug> slugs)
    {
        var slugValues = GetSlugValues(slugs, true);
        JsonHelpers.SerializeToFile(projectConfigFile, slugValues);
    }

    /// <summary>
    ///     Updates a template with user-defined slug values.
    /// </summary>
    /// <param name="projectConfigFile">The project config file.</param>
    /// <param name="template">The template.</param>
    /// <returns>The updated Template.</returns>
    /// <exception cref="Exception">Raises if we can't deserialize the project config file.</exception>
    private Template UpdateTemplateWithUserInput(string projectConfigFile, Template template)
    {
        if (this._specialValueHandler == null)
        {
            throw new Exception("This shouldn't happen...");
        }

        var slugs = template.Slugs;
        var outputSlugs = slugs.Where(x => !x.RequiresUserInput).ToList();
        var inputNeededSlugs = slugs.Where(x => x.RequiresUserInput).ToList();

        // Step 1 - Update non-editable slugs...
        for (var i = 0; i < outputSlugs.Count; i++)
        {
            outputSlugs[i] = this._specialValueHandler.AssignValuesToSlug(outputSlugs[i]);
        }

        // Step 2 - Update editable slugs...
        if (inputNeededSlugs.Count > 0)
        {
            inputNeededSlugs = GetUserInputForSlugs(projectConfigFile, inputNeededSlugs);
        }

        // Step 3 - Combine lists and output result...
        var finalSlugs = outputSlugs.CombineLists(inputNeededSlugs);
        template.Slugs = finalSlugs;

        var backupConfigFile = Path.Combine(PathConstants.ProjectGenerationConfigDirectory,
            $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}_{template.SafeName}.json");
        SaveSlugsToProjectConfigFile(projectConfigFile, inputNeededSlugs);
        SaveSlugsToProjectConfigFile(backupConfigFile, inputNeededSlugs);

        return template;
    }

    /// <summary>
    ///     Gets user input for slugs.
    /// </summary>
    /// <param name="projectConfigFile">The project config file.</param>
    /// <param name="slugs">The template's slugs.</param>
    /// <returns>The updated slugs.</returns>
    private List<Slug> GetUserInputForSlugs(string projectConfigFile, List<Slug> slugs)
    {
        if (this._specialValueHandler == null)
        {
            throw new Exception("This shouldn't happen...");
        }

        var projectConfigFileExists = File.Exists(projectConfigFile);
        if (projectConfigFileExists)
        {
            slugs = GetSlugsFromProjectConfigFile(projectConfigFile, slugs);
        }

        do
        {
            for (var i = 0; i < slugs.Count; i++)
            {
                slugs[i] = this._specialValueHandler.AssignValuesToSlug(slugs[i]);
                slugs[i] = UpdateSlugWithUserInput(slugs[i]);
            }
        } while (ContinueEditingSlugs(slugs, false));

        return slugs;
    }

    /// <summary>
    ///     Gets a user-defined value for a slug.
    /// </summary>
    /// <param name="slug">The slug.</param>
    /// <returns>The updated slug.</returns>
    private Slug UpdateSlugWithUserInput(Slug slug)
    {
        var continueEditingSlug = true;
        var displayMessage = $"{slug.DisplayName}";

        if (slug.AllowedValues.Count > 0)
        {
            var parts = slug.AllowedValues.Select(x => slug.Type.ObjectToString(x));
            displayMessage = $"{displayMessage} (allowed values: {string.Join(", ", parts)})";
        }

        if (slug.DisallowedValues.Count > 0)
        {
            var parts = slug.DisallowedValues.Select(x => slug.Type.ObjectToString(x));
            displayMessage = $"{displayMessage} (disallowed values: {string.Join(", ", parts)})";
        }

        displayMessage = $"{displayMessage}";

        do
        {
            var value = ConsoleHelpers.GetInput(displayMessage, slug.Type, slug.CurrentValue);

            if (slug.AllowedValues.Count > 0 && !slug.AllowedValues.Contains(value))
            {
                LogMessage("Value '{value}' is not allowed for this slug!");
                continue;
            }

            if (slug.DisallowedValues.Count > 0 && !slug.DisallowedValues.Contains(value))
            {
                LogMessage("Value '{value}' is not allowed for this slug!");
                continue;
            }

            slug.CurrentValue = value;
            continueEditingSlug = false;
        } while (continueEditingSlug);

        return slug;
    }

    /// <summary>
    ///     A helper method to ask if we need to continue editing slugs.
    /// </summary>
    /// <param name="slugs">The slugs.</param>
    /// <param name="defaultYes">Whether to default to continuing to edit or not.</param>
    /// <returns>True to continue to edit, False otherwise.</returns>
    private bool ContinueEditingSlugs(List<Slug> slugs, bool defaultYes = true)
    {
        var contents = JsonHelpers.SerializeToString(GetSlugValues(slugs, false));
        LogMessage($" {Environment.NewLine}CONFIGURATION{Environment.NewLine}{contents}");
        return ConsoleHelpers.GetYesNo("Edit Configuration?", defaultYes);
    }

    /// <summary>
    ///     Gets values for the slugs in a dictionary format.
    /// </summary>
    /// <param name="slugs">The slugs.</param>
    /// <param name="useSlugKeyOrDisplayName">True to use Slug.SlugKey, False to use Slug.DisplayName.</param>
    /// <returns>A dictionary representing the slugs.</returns>
    private Dictionary<string, object?> GetSlugValues(List<Slug> slugs, bool useSlugKeyOrDisplayName)
    {
        if (useSlugKeyOrDisplayName)
        {
            return slugs.ToDictionary(x => x.SlugKey, x => x.CurrentValue);
        }

        return slugs.ToDictionary(x => x.DisplayName, x => x.CurrentValue);
    }
}
