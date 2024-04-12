using CommandLine;
using ProjectTools.Core;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Helpers;

namespace ProjectTools.Options;

/// <summary>
/// A command to prepare a project/solution directory as a template
/// </summary>
/// <seealso cref="ProjectTools.Options.AbstractOption"/>
[Verb("prepare", HelpText = "Prepare a template")]
internal class PrepareTemplate : AbstractOption
{
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
    /// Gets or sets the type of the solution.
    /// </summary>
    /// <value>The type of the solution.</value>
    [Option('t', "type", Required = false, Default = "auto",
        HelpText = "The type of the solution to prepare. Defaults to auto.")]
    public string SolutionType { get; set; } = "auto";

    /// <summary>
    /// Gets or sets a value indicating whether [what if].
    /// </summary>
    /// <value><c>true</c> if [what if]; otherwise, <c>false</c>.</value>
    [Option('i', "what-if", Required = false, Default = false,
        HelpText =
            "If flag is provided, the template will not be prepared, but the user will be guided through all settings.")]
    public bool WhatIf { get; set; } = false;

    /// <summary>
    /// Executes what this option represents.
    /// </summary>
    /// <returns>The result of the execution.</returns>
    /// <exception cref="System.Exception">
    /// Silent is not a valid option for this command! or Directory specified does not exist! or Invalid solution
    /// type! Valid solution types are: auto, {string.Join(", ", validSolutionTypes)} or Could not detect valid implementation!
    /// </exception>
    public override string Execute()
    {
        // Validate parameters
        if (Silent) throw new Exception("Silent is not a valid option for this command!");

        if (!System.IO.Directory.Exists(Directory)) throw new Exception("Directory specified does not exist!");

        Implementation? implementation;
        var validSolutionTypes = Enum.GetNames(typeof(Implementation));
        if (SolutionType.ToLowerInvariant() == "auto")
            implementation = Manager.Instance.Templater.DetectValidImplementationForDirectory(Directory);
        else if (validSolutionTypes.Contains(SolutionType))
            implementation = (Implementation?)Enum.Parse<Implementation>(SolutionType);
        else
            throw new Exception(
                $"Invalid solution type! Valid solution types are: auto, {string.Join(", ", validSolutionTypes)}");
        if (implementation == null) throw new Exception("Could not detect valid implementation!");

        // Get the Templater
        var templater = Manager.Instance.Templater.GetAbstractTemplaterForImplementation(implementation.Value);

        // Request User Input and Get Template Generated
        var template = GetTemplateForPreparation(templater);

        // Prepare the template if we aren't in WhatIf mode
        if (WhatIf) return "Template not prepared, but configuration settings saved!";

        var prepareSettings = new PrepareOptions()
        {
            Directory = Directory, OutputDirectory = OutputDirectory, SkipCleaning = SkipCleaning, Template = template
        };
        var results = templater.PrepareTemplate(prepareSettings, LogMessage);
        return results;
    }

    /// <summary>
    /// Sets the options.
    /// </summary>
    /// <param name="option">The option.</param>
    protected override void SetOptions(AbstractOption option)
    {
        var options = (PrepareTemplate)option;
        Directory = options.Directory;
        OutputDirectory = options.OutputDirectory;
        SkipCleaning = options.SkipCleaning;
        SolutionType = options.SolutionType;
        WhatIf = options.WhatIf;
        Silent = options.Silent;
    }

    /// <summary>
    /// Gets the template for preparation.
    /// </summary>
    /// <param name="templater">The templater.</param>
    /// <returns>The template the user is building.</returns>
    /// <exception cref="System.Exception">Could not find a value for field {setting.Metadata.RequiredFieldName}!</exception>
    private Template GetTemplateForPreparation(AbstractTemplater templater)
    {
        Template template;
        // get preperation settings we need user input for
        var templateSettingsFile = Path.Combine(Directory, Constants.TemplaterTemplatesInfoFileName);
        var (settingsNeeded, hadFile) = templater.GetPreperationUserSettings(templateSettingsFile);

        // if we had an existing file, check with the user if they even want to modify the settings or not
        if (hadFile)
        {
            _ = LogMessage($" {Environment.NewLine}TEMPLATE INFORMATION");
            if (!PropertyHelpers.ContinueEditingSettings(settingsNeeded, LogMessage))
            {
                template = templater.GetTemplateForProperties(settingsNeeded, Directory, true);
                return template;
            }
        }

        var specialOptions = string.Join(", ", Constants.SPECIAL_TEXT);
        // if we didn't have an existing file, or the user wants to edit the settings, get the user input
        bool continueEditing;
        do
        {
            _ = LogMessage($" {Environment.NewLine}TEMPLATE INFORMATION");
            _ = LogMessage($"Special Text: {specialOptions}");

            // loop through each setting we need and ask what it should be
            foreach (var setting in settingsNeeded)
            {
                // if this setting requires another setting to be a certain value, check that value
                if (!string.IsNullOrWhiteSpace(setting.Metadata.RequiredFieldName))
                {
                    var otherField = settingsNeeded.Where(x => x.Name == setting.Metadata.RequiredFieldName)
                        .FirstOrDefault();
                    var otherFieldValue = otherField?.CurrentValue;

                    if (otherFieldValue == null)
                        throw new Exception($"Could not find a value for field {setting.Metadata.RequiredFieldName}!");

                    var actualOtherValue = PropertyHelpers.GetDisplayValue(otherFieldValue, otherField.Type);
                    var requiredOtherValue =
                        PropertyHelpers.GetDisplayValue(setting.Metadata.RequiredFieldValue, otherField.Type);
                    var disallowedOtherValue =
                        PropertyHelpers.GetDisplayValue(setting.Metadata.DisallowedFieldValue, otherField.Type);
                    if (actualOtherValue != requiredOtherValue || actualOtherValue == disallowedOtherValue) continue;
                }

                // otherwise, get the user input for the setting
                var result = PropertyHelpers.GetInputForProperty(setting);
                setting.CurrentValue = result;
            }

            _ = LogMessage(" ");
            continueEditing = PropertyHelpers.ContinueEditingSettings(settingsNeeded, LogMessage);
        } while (continueEditing);

        template = templater.GetTemplateForProperties(settingsNeeded, Directory, true);
        return template;
    }
}