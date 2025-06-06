#region

using CommandLine;
using ProjectTools.CL.Helpers;
using ProjectTools.CL.MenuSystem;
using ProjectTools.Core;
using ProjectTools.Core.Constants;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.TemplateBuilders;
using ProjectTools.Core.Templates;

#endregion

namespace ProjectTools.CL.Options;

/// <summary>
///     A command to prepare a project/solution directory as a template
/// </summary>
/// <seealso cref="AbstractOption" />
[Verb("prepare", HelpText = "Prepare a new template.")]
[MenuMetadata(80)]
public class PrepareTemplate : AbstractOption
{
    /// <summary>
    ///     A dictionary containing the special keys for each field
    /// </summary>
    private readonly Dictionary<SlugType, string> _messageExtras = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="PrepareTemplate" /> class.
    /// </summary>
    public PrepareTemplate()
    {
        this.AllowTemplateUpdates = false;

        var specialValueHandler = new SpecialValueHandler("C://my_fake_directory//sub_directory", "fake_proj", null);

        foreach (var slugType in Enum.GetValues<SlugType>())
        {
            var messageExtra = "";
            var specialKeywords = specialValueHandler.GetSpecialKeywords(slugType);
            if (specialKeywords.Count > 0)
            {
                messageExtra = $" (special keywords: {string.Join(", ", specialKeywords)})";
            }

            this._messageExtras.Add(slugType, messageExtra);
        }
    }

    /// <summary>
    ///     Gets or sets the directory.
    /// </summary>
    [Option('d', "directory", Required = true, HelpText = "The directory to prepare as a template.")]
    public string Directory { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a value indicating whether [force override].
    /// </summary>
    [Option('f', "force", Required = false, Default = false,
        HelpText = "If flag is provided, any existing template will be overriden.")]
    public bool ForceOverride { get; set; }

    /// <summary>
    ///     Gets or sets the output directory.
    /// </summary>
    [Option('o', "output-directory", Required = true, HelpText = "The output directory to place the template into.")]
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a value indicating whether [skip cleaning].
    /// </summary>
    [Option('c', "skip-cleaning", Required = false, Default = false,
        HelpText = "If flag is provided, the working directory won't be deleted at the end of the prepare process.")]
    public bool SkipCleaning { get; set; }

    /// <summary>
    ///     Gets or sets the template builder to use with the solution.
    /// </summary>
    [Option('t', "template-builder", Required = false, Default = "auto",
        HelpText = "The type of template builder to use to prepare the template. Defaults to auto.")]
    public string TemplateBuilder { get; set; } = "auto";

    /// <summary>
    ///     Gets or sets a value indicating whether [what if].
    /// </summary>
    [Option('i', "what-if", Required = false, Default = false,
        HelpText =
            "If flag is provided, the template will not be prepared, but the user will be guided through all settings.")]
    public bool WhatIf { get; set; }

    /// <summary>
    ///     A helper to determine if we should continue editing a slug or not.
    /// </summary>
    /// <param name="slug">The slug.</param>
    /// <param name="defaultYes">True to say we want to continue editing the slug, False otherwise.</param>
    /// <returns>True if we want to edit the slug, False otherwise.</returns>
    private bool ContinueEditingSlug(PreparationSlug slug, bool defaultYes = true)
    {
        var contents = JsonHelpers.SerializeToString(slug);
        LogMessage($" {Environment.NewLine}SLUG{Environment.NewLine}{contents}");
        return ConsoleHelpers.GetYesNo("Edit slug?", defaultYes);
    }

    /// <summary>
    ///     A helper to determine if we should continue editing slugs or not.
    /// </summary>
    /// <param name="slugs">The slugs.</param>
    /// <param name="defaultYes">True to say we want to continue editing the slugs, False otherwise.</param>
    /// <returns>True if we want to edit the slugs, False otherwise.</returns>
    private bool ContinueEditingSlugs(List<PreparationSlug> slugs, bool defaultYes = true)
    {
        var contents = JsonHelpers.SerializeToString(slugs);
        LogMessage($" {Environment.NewLine}SLUGS{Environment.NewLine}{contents}");
        return ConsoleHelpers.GetYesNo("Edit slugs?", defaultYes);
    }

    private bool ContinueEditingTemplateSettings(PreparationTemplate template, bool defaultYes = true)
    {
        var contents = JsonHelpers.SerializeToString(template.GetTemplateWithoutSlugs());
        LogMessage($" {Environment.NewLine}TEMPLATE INFORMATION{Environment.NewLine}{contents}");
        return ConsoleHelpers.GetYesNo("Edit template settings?", defaultYes);
    }

    /// <summary>
    ///     Executes the option.
    /// </summary>
    /// <returns>The result.</returns>
    public override string Execute()
    {
        // Validate parameters
        ValidateParameters();

        // Get the template builder for the preparation of the directory
        var templater = new Preparer();
        var templateBuilderForPrep = GetTemplateBuilderForPreparation(templater);

        // Get info on the template we're building...
        var (template, hadExistingTemplate) = GetTemplateForDirectory(templateBuilderForPrep);

        // Get the preparation slugs for the template we're building...
        var prepSlugs = templateBuilderForPrep.GetPreparationSlugs(this.Directory, template);

        // Ask questions about the preparation slugs that we need to know...
        template.Slugs = GetSlugs(prepSlugs, hadExistingTemplate);

        // Save the template info to the directory...
        var sourceTemplateInfoFile = Path.Combine(this.Directory, TemplateConstants.TemplateSettingsFileName);
        IOHelpers.DeleteFileIfExists(sourceTemplateInfoFile);
        JsonHelpers.SerializeToFile(sourceTemplateInfoFile, template);

        // Prepare the directory!
        if (this.WhatIf)
        {
            return "What-If mode enabled. No changes were made.";
        }

        Logger coreLogger = new(LogMessage);
        return templater.GenerateTemplate(this.Directory, this.OutputDirectory, this.SkipCleaning, this.ForceOverride,
            template, coreLogger);
    }

    /// <summary>
    ///     Gets all existing slugs that require input.
    /// </summary>
    /// <param name="slugs">The slugs.</param>
    /// <returns>The slugs.</returns>
    private List<PreparationSlug> GetBuiltinSlugs(List<PreparationSlug> slugs)
    {
        do
        {
            for (var i = 0; i < slugs.Count; i++)
            {
                slugs[i] = GetSlugInfo(slugs[i], false);
                // PRINT LINE
                ConsoleHelpers.PrintLine();
            }
        } while (ContinueEditingSlugs(slugs, false));

        return slugs;
    }

    /// <summary>
    ///     Gets all existing slugs that require input.
    /// </summary>
    /// <param name="slugs">The slugs.</param>
    /// <returns>The slugs.</returns>
    private List<PreparationSlug> GetCustomOldSlugs(List<PreparationSlug> slugs)
    {
        do
        {
            for (var i = 0; i < slugs.Count; i++)
            {
                slugs[i] = GetSlugInfo(slugs[i], true);
                // PRINT LINE
                ConsoleHelpers.PrintLine();
            }
        } while (ContinueEditingSlugs(slugs, false));

        return slugs;
    }

    /// <summary>
    ///     Gets custom slugs.
    /// </summary>
    /// <returns>The slugs.</returns>
    private List<PreparationSlug> GetCustomSlugs()
    {
        List<PreparationSlug> customSlugs = new();

        while (ConsoleHelpers.GetYesNo("Do you want to add any additional slugs?", false))
        {
            PreparationSlug slug = new();
            slug = GetSlugInfo(slug, true);

            customSlugs.Add(slug);
        }

        return customSlugs;
    }

    /// <summary>
    ///     Gets info from the user about a slug.
    /// </summary>
    /// <param name="slug">The slug.</param>
    /// <param name="allowAdjustSlugType">True to allow a user to change the slug type, False otherwise.</param>
    /// <returns>The updated slug.</returns>
    private PreparationSlug GetSlugInfo(PreparationSlug slug, bool allowAdjustSlugType)
    {
        var isValidReason = string.Empty;
        do
        {
            // Slug Display Name
            slug.DisplayName = ConsoleHelpers.GetInput("Slug Display Name", slug.DisplayName);

            // Slug Key
            slug.SlugKey = ConsoleHelpers.GetInput("Slug Key", slug.SlugKey);

            // Slug Type
            if (allowAdjustSlugType)
            {
                slug.Type = ConsoleHelpers.GetEnumInput("Slug Type", slug.Type);
            }

            // Slug Search Strings
            slug.SearchStrings = ConsoleHelpers.GetStringListInput("Slug Search Strings", slug.SearchStrings,
                ",", "comma-separated");

            // Random Guids don't need any other info...
            if (slug.Type == SlugType.RandomGuid)
            {
                slug.DefaultValue = null;
                slug.AllowedValues = [];
                slug.DisallowedValues = [];
                slug.RequiresUserInput = false;
            }
            else
            {
                // Slug Default Value, if any
                var defaultValueDisplay = slug.DefaultValue ?? string.Empty;
                var messageExtra = this._messageExtras[slug.Type];

                var defaultValue =
                    ConsoleHelpers.GetInput($"Slug Default Value{messageExtra}", defaultValueDisplay);
                slug.DefaultValue = defaultValue;

                slug.Description = ConsoleHelpers.GetInput("Slug Description", slug.Description);

                // Whether the slug allows empty values or not
                slug.AllowEmptyValues = ConsoleHelpers.GetYesNo("Allow empty values?", slug.AllowEmptyValues);

                if (slug.Type == SlugType.String)
                {
                    // Slug Allowed Values, if any
                    var allowedValues = ConsoleHelpers.GetStringListInput("Slug Allowed Values", slug.AllowedValues,
                        ",", "comma-separated");
                    slug.AllowedValues = allowedValues;

                    // Whether the slug is case-sensitive or not
                    slug.CaseSensitive = ConsoleHelpers.GetYesNo("Is the slug case-sensitive?", slug.CaseSensitive);

                    // Slug Disallowed Values, if any
                    var disallowedValues = ConsoleHelpers.GetStringListInput("Slug Disallowed Values",
                        slug.DisallowedValues,
                        ",", "comma-separated");
                    slug.DisallowedValues = disallowedValues;
                }

                // Slug Requires User Input
                slug.RequiresUserInput =
                    ConsoleHelpers.GetYesNo("Does the slug require user input?", slug.RequiresUserInput);
            }

            try
            {
                slug.Validate();
            }
            catch (Exception ex)
            {
                isValidReason = ex.ToString();
                LogMessage($"Invalid Slug: {isValidReason}");
            }
        } while (ContinueEditingSlug(slug, false) || !string.IsNullOrWhiteSpace(isValidReason));

        return slug;
    }

    private List<PreparationSlug> GetSlugs(List<PreparationSlug> prepSlugs, bool hadExistingTemplate)
    {
        // Get the slugs that don't require any input and those that do
        var slugsWithNoInput = prepSlugs.Where(s => !s.RequiresAnyInput).ToList();
        var slugsWithInput = prepSlugs.Where(s => s.RequiresAnyInput).ToList();
        var slugsWithInputBuiltIn = prepSlugs.Where(s => s.RequiresAnyInput && !s.CustomSlug).ToList();
        var slugsWithInputCustom = prepSlugs.Where(s => s.RequiresAnyInput && s.CustomSlug).ToList();

        // If we had an existing template file, we may have slug info the user may want to stick with...
        if (hadExistingTemplate)
        {
            if (!ContinueEditingSlugs(slugsWithInput, false))
            {
                return slugsWithNoInput.CombineLists(slugsWithInput);
            }
        }

        // Ask the user for input on the slugs that require it...
        slugsWithInputBuiltIn = GetBuiltinSlugs(slugsWithInputBuiltIn);
        slugsWithInputCustom = GetCustomOldSlugs(slugsWithInputCustom);

        // Ask if the user has any additional slugs they want to add...
        var customSlugs = GetCustomSlugs();

        // Combine the three lists and return!
        var slugs = slugsWithNoInput.CombineLists(slugsWithInputBuiltIn).CombineLists(slugsWithInputCustom)
            .CombineLists(customSlugs);
        return slugs;
    }

    /// <summary>
    ///     Gets the template builder for the preparation of the directory.
    /// </summary>
    /// <param name="preparer">The templater engine.</param>
    /// <returns>A template builder for use in preparation.</returns>
    /// <exception cref="Exception">Raises if no template builder could be used.</exception>
    private AbstractTemplateBuilder GetTemplateBuilderForPreparation(Preparer preparer)
    {
        var templateBuilderForPrep = preparer.GetTemplateBuilderForOption(this.TemplateBuilder, this.Directory);
        return templateBuilderForPrep;
    }

    private (PreparationTemplate, bool) GetTemplateForDirectory(AbstractTemplateBuilder templateBuilder)
    {
        // Get the existing template settings file, if it exists
        var templateSettingsFile = Path.Combine(this.Directory, TemplateConstants.TemplateSettingsFileName);
        var template = JsonHelpers.DeserializeFromFile<PreparationTemplate>(templateSettingsFile);
        if (template != null)
        {
            if (template.TemplaterVersion < TemplateConstants.MinSupportedTemplateVersion ||
                template.TemplaterVersion > TemplateConstants.MaxSupportedTemplateVersion)
            {
                LogMessage(
                    $"Template version {template.TemplaterVersion} is not supported. Current supported versions are {TemplateConstants.MinSupportedTemplateVersion} to {TemplateConstants.MaxSupportedTemplateVersion}");
                template = null;
            }
        }

        var hadExistingSettingsFile = template != null;
        template ??= new PreparationTemplate();
        template.TemplaterVersion = TemplateConstants.CurrentTemplateVersion;

        // Populate the template builder if it's not already populated
        if (string.IsNullOrEmpty(template.TemplateBuilder))
        {
            template.TemplateBuilder = templateBuilder.NameLowercase;
        }

        // if we had an existing template file, ask if the user wants to modify it
        if (hadExistingSettingsFile)
        {
            if (!ContinueEditingTemplateSettings(template, false))
            {
                return (template, hadExistingSettingsFile);
            }
        }

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

            // Ask for PrepareScripts
            template.PrepareScripts = ConsoleHelpers.GetStringListInput("Template Preparation Scripts",
                template.PrepareScripts,
                ",", "comma-separated");

            // Ask for RenameOnlyPaths
            template.RenameOnlyPaths = ConsoleHelpers.GetStringListInput("Rename-Only Paths", template.RenameOnlyPaths,
                ",", "comma-separated");

            // Ask for PathsToRemove
            template.PathsToRemove =
                ConsoleHelpers.GetStringListInput("Paths to Remove", template.PathsToRemove, ",", "comma-separated");

            // Ask for PrepareExcludedPaths
            template.PrepareExcludedPaths =
                ConsoleHelpers.GetStringListInput("Paths Excluded From Template", template.PrepareExcludedPaths, ",",
                    "comma-separated");

            // Ask for PythonScriptPaths
            template.PythonScriptPaths = ConsoleHelpers.GetStringListInput("Python Script Paths",
                template.PythonScriptPaths, ",", "comma-separated");

            // Ask for Instructions
            template.Instructions =
                ConsoleHelpers.GetStringListInput("User Instructions", template.Instructions, ",", "comma-separated");
        } while (ContinueEditingTemplateSettings(template, false));

        return (template, hadExistingSettingsFile);
    }

    /// <summary>
    ///     Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (PrepareTemplate)option;
        this.Directory = options.Directory;
        this.OutputDirectory = options.OutputDirectory;
        this.SkipCleaning = options.SkipCleaning;
        this.TemplateBuilder = options.TemplateBuilder.ToLowerInvariant();
        this.WhatIf = options.WhatIf;
        this.ForceOverride = options.ForceOverride;
    }

    /// <summary>
    ///     Validates the command parameters.
    /// </summary>
    /// <exception cref="Exception">Raises if a parameter is invalid.</exception>
    private void ValidateParameters()
    {
        if (!System.IO.Directory.Exists(this.Directory))
        {
            throw new Exception("Directory specified does not exist!");
        }
    }
}
