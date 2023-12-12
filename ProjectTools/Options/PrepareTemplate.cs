using System.Text;
using System.Text.Json;
using CommandLine;
using ProjectTools.Core;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Core.Templating.Preparation;
using ProjectTools.Helpers;

namespace ProjectTools.Options
{
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
        [Option('d', "directory", Required = true, HelpText = "The directory to prepare as a template")]
        public string Directory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        [Option('o', "output-directory", Required = true, HelpText = "The output directory to place the template into")]
        public string OutputDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether [skip cleaning].
        /// </summary>
        /// <value><c>true</c> if [skip cleaning]; otherwise, <c>false</c>.</value>
        [Option(
            's',
            "skip-cleaning",
            Required = false,
            Default = false,
            HelpText = "If flag is provided, the working directory won't be deleted at the end of the prepare process."
               )]
        public bool SkipCleaning { get; set; } = false;

        /// <summary>
        /// Gets or sets the type of the solution.
        /// </summary>
        /// <value>The type of the solution.</value>
        [Option(
            't',
            "type",
            Required = false,
            Default = "auto",
            HelpText = "The type of the solution to prepare. Defaults to auto."
               )]
        public string SolutionType { get; set; } = "auto";

        /// <summary>
        /// Gets or sets a value indicating whether [what if].
        /// </summary>
        /// <value><c>true</c> if [what if]; otherwise, <c>false</c>.</value>
        [Option(
            'i',
            "what-if",
            Required = false,
            Default = false,
            HelpText = "If flag is provided, the template will not be prepared, but the user will be guided through all settings."
               )]
        public bool WhatIf { get; set; } = false;

        /// <summary>
        /// Executes what this option represents.
        /// </summary>
        /// <returns>The result of the execution.</returns>
        /// <exception cref="System.Exception">
        /// Silent is not a valid option for this command! or Directory specified does not exist! or Invalid solution
        /// type! Valid solution types are: auto, {string.Join(", ", validSolutionTypes)} or Could not detect valid implementation!
        /// </exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string Execute()
        {
            // Validate parameters
            if (Silent)
            {
                throw new Exception("Silent is not a valid option for this command!");
            }

            if (!System.IO.Directory.Exists(Directory))
            {
                throw new Exception("Directory specified does not exist!");
            }

            Implementation? implementation;
            var validSolutionTypes = Enum.GetNames(typeof(Implementation));
            if (SolutionType.ToLowerInvariant() == "auto")
            {
                implementation = Manager.Instance.Templater.DetectValidImplementation(Directory);
            }
            else if (validSolutionTypes.Contains(SolutionType))
            {
                implementation = (Implementation?)Enum.Parse<Implementation>(SolutionType);
            }
            else
            {
                throw new Exception(
                    $"Invalid solution type! Valid solution types are: auto, {string.Join(", ", validSolutionTypes)}"
                                   );
            }
            if (implementation == null)
            {
                throw new Exception("Could not detect valid implementation!");
            }

            // Get the template preparer
            var templatePreparer = Manager.Instance.Templater.GetTemplatePreparer(implementation.Value);

            // setup the template + default settings
            var coreSettings = GetCoreSettings(templatePreparer);
            var settings = GetSettings(templatePreparer);
            coreSettings.Settings = settings;

            // Prepare the template if not in what-if mode
            if (WhatIf)
            {
                return "Template not prepared, but configuration settings saved!";
            }

            var prepareSettings = new PrepareOptions() { Directory = Directory, OutputDirectory = OutputDirectory, SkipCleaning = SkipCleaning, TemplateSettings = coreSettings };
            var results = templatePreparer.Prepare(prepareSettings, LogMessage);

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
        /// Continues the editing settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>True to edit, False to finish editing</returns>
        private bool ContinueEditingSettings(Dictionary<SettingProperty, object> settings, string editText)
        {
            var displayedSettings = settings.Select(x => (x.Key.Order, $"{x.Key.DisplayName}: {GetDisplayValue(x.Value, x.Key.Type)}{Environment.NewLine}")).OrderBy(x => x.Order).ToList();

            var sb = new StringBuilder();
            foreach (var setting in displayedSettings)
            {
                _ = sb.Append($"    {setting.Item2}");
            }

            var result = ConsoleHelpers.GetYesNo(
                $"{editText}?{Environment.NewLine}{sb}{Environment.NewLine}",
                false
                                                );
            LogMessage(" ");
            return !result;
        }

        /// <summary>
        /// Gets core template information
        /// </summary>
        /// <param name="templatePreparer">The template preparer.</param>
        /// <returns>An abstract template instance with core info populated</returns>
        /// <exception cref="Exception">No template instance could be created</exception>
        private AbstractTemplate GetCoreSettings(AbstractTemplatePreparer templatePreparer)
        {
            var templateSettingsFile = Path.Combine(Directory, Core.Constants.TemplaterTemplatesInfoFileName);
            (var settingsNeeded, var hadFile) = templatePreparer.GetTemplateProperties(templateSettingsFile);
            var rawSettings = RequestTemplateSettings(settingsNeeded, hadFile, "TEMPLATE INFORMATION", "Edit Core Template Information", false);

            var settings = templatePreparer.GetTemplateInstance(rawSettings);
            if (settings == null)
            {
                throw new Exception("No template found!");
            }

            SaveFile(templateSettingsFile, settings);
            return settings;
        }

        /// <summary>
        /// Gets the display value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The string representation</returns>
        private string GetDisplayValue(object? value, SettingType type)
        {
            return type switch
            {
                SettingType.Bool => (bool?)value ?? false ? "Yes" : "No",
                SettingType.String => (value ?? string.Empty).ToString(),
                SettingType.StringListComma => string.Join(", ", (value as List<string>) ?? []),
                SettingType.StringListSemiColan => string.Join("; ", (value as List<string>) ?? []),
                SettingType.DictionaryStringString
                    => string.Join(
                        ", ",
                        ((value as Dictionary<string, string>) ?? [])
                            .Select(x => string.Join(": ", x.Key, x.Value))
                            .ToList()
                                  ),
                _ => throw new Exception($"Unknown setting type {type}!"),
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private object GetInputForProperty(SettingProperty setting, object currentValue)
        {
            object response;
            string tempResponse;
            List<string> tempListResponse;
            var defaultValue = GetDisplayValue(currentValue, setting.Type);
            switch (setting.Type)
            {
                case SettingType.Bool:
                    response = ConsoleHelpers.GetYesNo(setting.DisplayName, (bool)currentValue);
                    break;

                case SettingType.String:
                    response = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    break;

                case SettingType.StringListComma:
                    tempResponse = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    tempListResponse = tempResponse.Split(",").Select(x => x.Trim()).ToList();
                    response = tempListResponse.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                    break;

                case SettingType.StringListSemiColan:
                    tempResponse = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    tempListResponse = tempResponse.Split(";").Select(x => x.Trim()).ToList();
                    response = tempListResponse.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                    break;

                case SettingType.DictionaryStringString:
                    tempResponse = ConsoleHelpers.GetInput(setting.DisplayName, defaultValue);
                    tempListResponse = tempResponse.Split(",").Select(x => x.Trim()).ToList();
                    response = tempListResponse.Where(x => !string.IsNullOrWhiteSpace(x)).ToDictionary(x => x.Split(":")[0].Trim(), x => x.Split(":")[1].Trim());
                    break;

                default:
                    throw new Exception($"Unknown setting type {setting.Type}!");
            }

            return response;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private TemplateSettings GetSettings(AbstractTemplatePreparer templatePreparer)
        {
            var templateSettingsFile = Path.Combine(Directory, Core.Constants.TemplaterTemplatesSettingsFileName);
            (var settingsNeeded, var hadFile) = templatePreparer.GetSettingProperties(templateSettingsFile);
            var rawSettings = RequestTemplateSettings(settingsNeeded, hadFile, "TEMPLATE SETTINGS", "Edit Settings");

            var settings = templatePreparer.GetSettingsClassForProperties(rawSettings);
            if (settings == null)
            {
                throw new Exception("No settings found!");
            }

            SaveFile(templateSettingsFile, settings);
            return settings;
        }

        /// <summary>
        /// Requests the template settings.
        /// </summary>
        /// <param name="settingsNeeded">The settings needed.</param>
        /// <returns>A list of settings for this template</returns>
        private Dictionary<SettingProperty, object> RequestTemplateSettings(
            List<SettingProperty> settingsNeeded,
            bool hadFile,
            string categoryText,
            string editText,
            bool displaySpecialText = true
                                                                           )
        {
            // populate settings dict with defaults
            Dictionary<SettingProperty, object> output = [];
            foreach (var setting in settingsNeeded)
            {
                output.Add(setting, setting.CurrentValue);
            }

            // if we had a file, first ask if the existing settings look fine
            if (hadFile)
            {
                LogMessage($" {Environment.NewLine}{categoryText}");
                if (ContinueEditingSettings(output, editText))
                {
                    return output;
                }
            }

            var continueEditing = false;
            // loop through getting settings as needed
            do
            {
                LogMessage($" {Environment.NewLine}{categoryText}");
                if (displaySpecialText)
                {
                    LogMessage("Special Text: <CurrentUserName>, <ParentDir>, <ProjectName>");
                }
                for (var i = 0; i < output.Count; i++)
                {
                    (var setting, var currentValue) = output.Where(x => x.Key.Order == i).First();
                    output[setting] = GetInputForProperty(setting, currentValue);
                }
                LogMessage(" ");
                continueEditing = ContinueEditingSettings(output, editText);
            } while (!continueEditing);

            return output;
        }

        /// <summary>
        /// Saves the file with the contents.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="obj"></param>
        private void SaveFile(string file, object obj)
        {
            var contents = JsonSerializer.Serialize(obj, Constants.JsonSerializeOptions);
            File.WriteAllText(file, contents);
        }
    }
}
