using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templaters;
using ProjectTools.Core.Templates;

namespace ProjectTools.CL.Options;

/// <summary>
/// A command to prepare a project/solution directory as a template
/// </summary>
/// <seealso cref="AbstractOption"/>
[Verb("prepare", HelpText = "Prepare a template")]
public class PrepareTemplate : AbstractOption
{
    public PrepareTemplate()
    {
        AllowSilentParameter = false;
    }

    /// <summary>
    /// Gets or sets the directory.
    /// </summary>
    /// <value>The directory.</value>
    [Option('d', "directory", Required = true, HelpText = "The directory to prepare as a template.")]
    public string Directory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the output directory.
    /// </summary>
    /// <value>The output directory.</value>
    [Option('o', "output-directory", Required = true, HelpText = "The output directory to place the template into.")]
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether [skip cleaning].
    /// </summary>
    /// <value><c>true</c> if [skip cleaning]; otherwise, <c>false</c>.</value>
    [Option('c', "skip-cleaning", Required = false, Default = false,
        HelpText = "If flag is provided, the working directory won't be deleted at the end of the prepare process.")]
    public bool SkipCleaning { get; set; } = false;

    /// <summary>
    /// Gets or sets the template builder to use with the solution.
    /// </summary>
    /// <value>The template builder for the solution.</value>
    [Option('t', "type", Required = false, Default = "auto",
        HelpText = "The type of the solution to prepare. Defaults to auto.")]
    public string TemplateBuilder { get; set; } = "auto";

    /// <summary>
    /// Gets or sets a value indicating whether [what if].
    /// </summary>
    /// <value><c>true</c> if [what if]; otherwise, <c>false</c>.</value>
    [Option('i', "what-if", Required = false, Default = false,
        HelpText =
            "If flag is provided, the template will not be prepared, but the user will be guided through all settings.")]
    public bool WhatIf { get; set; } = false;

    protected override void SetOptions(AbstractOption option)
    {
        var options = (PrepareTemplate)option;
        Directory = options.Directory;
        OutputDirectory = options.OutputDirectory;
        SkipCleaning = options.SkipCleaning;
        TemplateBuilder = options.TemplateBuilder.ToLowerInvariant();
        WhatIf = options.WhatIf;
        Silent = options.Silent;
    }

    public override string Execute()
    {
        // Validate parameters
        ValidateParameters();

        // Get the template builder for the preparation of the directory
        var templater = new Templater();
        var templateBuilderForPrep = GetTemplateBuilderForPreparation();

        // Get info on the template we're building...
        var template = GetTemplateForDirectory(templateBuilderForPrep);

        // Get the preperation slugs for the template we're building...
    }

    /// <summary>
    /// Validates the command parameters.
    /// </summary>
    /// <exception cref="Exception">Raises if a parameter is invalid.</exception>
    private void ValidateParameters()
    {
        if (!System.IO.Directory.Exists(Directory)) throw new Exception("Directory specified does not exist!");
    }

    /// <summary>
    /// Gets the template builder for the preparation of the directory.
    /// </summary>
    /// <param name="templater">The templater engine.</param>
    /// <returns>A template builder for use in preperation.</returns>
    /// <exception cref="Exception">Raises if no template builder could be used.</exception>
    private AbstractTemplateBuilder GetTemplateBuilderForPreparation(Templater templater)
    {
        var templateBuilders = templater.GetTemplateBuilders();
        AbstractTemplateBuilder? templateBuilderForPrep = null;
        // If template builder is auto, try to detect the correct one
        if (TemplateBuilder == "auto")
        {
            foreach (var templateBuilder in templateBuilders)
                if (templateBuilder.IsValidDirectoryForBuilder(Directory))
                {
                    templateBuilderForPrep = templateBuilder;
                    break;
                }
        }
        // Otherwise, try to find the template builder by name...
        else
        {
            var templateBuilder = templateBuilders.Where(x => x.NameLowercase == TemplateBuilder).FirstOrDefault();
            if (templateBuilder != null)
                if (templateBuilder.IsValidDirectoryForBuilder(Directory))
                    templateBuilderForPrep = templateBuilder;
        }

        // Raise an exception if we couldn't find a valid template builder, otherwise return the template builder
        if (templateBuilderForPrep == null)
            throw new Exception("Could not detect valid template builder for directory!");

        return templateBuilderForPrep;
    }

    private Template GetTemplateForDirectory(AbstractTemplateBuilder templateBuilder)
    {
        // Get the existing template settings file, if it exists
        var templateSettingsFile = Path.Combine(Directory, TemplateConstants.TemplateSettingsFileName);
        var template = JsonHelpers.DeserializeFromFile<Template>(templateSettingsFile);

        var hadExistingSettingsFile = template == null;
        if (template == null) template = new Template();

        // Populate the template builder if it's not already populated
        if (string.IsNullOrEmpty(template.TemplateBuilder)) template.TemplateBuilder = templateBuilder.NameLowercase;

        // if we had an existing template file, ask if the user wants to modify it
        if (hadExistingSettingsFile)
            if (!ContinueEditingTemplateSettings(template, False))
                return template;

        // Ask user to inputs!
        do
        {
            // Ask for Name
            template.Name = ConsoleHelpers.GetInput("Template Name", template.Name);

            // Ask for Version
            template.Version = ConsoleHelpers.GetInput("Template Version", template.Version);

            // Ask for Description
            template.Description = ConsoleHelpers.GetInput("Template Description", template.Description);

            // Ask for Author
            template.Author = ConsoleHelpers.GetInput("Template Author", template.Author);

            // Ask for RenameOnlyPaths
            template.RenameOnlyPaths = ConsoleHelpers.GetStringListInput("Rename-Only Paths", template.RenameOnlyPaths,
                ",", "comma-separated");

            // Ask for PathsToRemove
            template.PathsToRemove =
                ConsoleHelpers.GetStringListInput("Paths to Remove", template.PathsToRemove, ",", "comma-separated");

            // Ask for PythonScriptPaths
            template.PythonScriptPaths = ConsoleHelpers.GetStringListInput("Python Script Paths",
                template.PythonScriptPaths, ",", "comma-separated");
        } while (ContinueEditingTemplateSettings(template));

        private bool ContinueEditingTemplateSettings(Template template, bool defaultYes = true)
        {
            var contents = JsonHelpers.SerializeToString(template);
            LogMessage($" {Environment.NewLine}TEMPLATE INFORMATION");
            return ConsoleHelpers.GetYesNo("Edit template settings?", defaultYes);
        }
    }
}
