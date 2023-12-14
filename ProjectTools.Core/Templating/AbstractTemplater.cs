using System.Diagnostics;
using ProjectTools.Core.Helpers;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Options;
using ProjectTools.Core.PropertyHelpers;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Core.Templating.Generation;

namespace ProjectTools.Core.Templating
{
    /// <summary>
    /// Defines an abstract templater implementation.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    public abstract class AbstractTemplater
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTemplater"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="implementation">The implementation.</param>
        public AbstractTemplater()
        {
            var implementationInfo = GetType().GetCustomAttributes(typeof(ImplementationRegister), false);

            if (implementationInfo == null || implementationInfo.Length == 0)
            {
                throw new Exception("A templater implementation must have an ImplementationRegister attribute!");
            }

            var metadata = (ImplementationRegister)implementationInfo[0];

            DisplayName = metadata.DisplayName;
            Implementation = metadata.Implementation;
            Description = metadata.Description;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <value>The implementation.</value>
        public Implementation Implementation { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => Implementation.ToString().ToLowerInvariant();

        /// <summary>
        /// Gets the type of the solution settings.
        /// </summary>
        /// <value>The type of the solution settings.</value>
        public abstract Type SolutionSettingsType { get; }

        /// <summary>
        /// Gets the type of the template information class for this implementation.
        /// </summary>
        /// <value>The type of the template information.</value>
        public abstract Type TemplateInformationType { get; }

        /// <summary>
        /// Gets the type of the template settings class for this implementation.
        /// </summary>
        /// <value>The type of the template settings.</value>
        public abstract Type TemplateSettingsType { get; }

        /// <summary>
        /// Returns whether the specified directory is valid to be prepared into a template for this implementation.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>True if valid, False otherwise.</returns>
        public abstract bool DirectoryValidForTemplatePreperation(string directory);

        /// <summary>
        /// Generates the project.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="log">The log.</param>
        /// <returns>The result of the generation</returns>
        public abstract string GenerateProject(GenerateOptions options, Func<string, bool> log, Func<string, bool> instructionLog, Func<string, bool> commandLog);

        /// <summary>
        /// Gets the generation solution setting properties.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="outputDir">The output dir.</param>
        /// <param name="solutionConfigFile">The solution configuration file.</param>
        /// <returns>Item 1: The properties we need user information on. Item 2: Whether an existing file was loaded</returns>
        public (List<Property> Properties, bool HadFile) GetGenerationSolutionSettingProperties(string templateName, string outputDir, string solutionConfigFile, string solutionName)
        {
            SolutionSettings? existingSettings = null;
            if (File.Exists(solutionConfigFile))
            {
                try
                {
                    // First, lets see if there is an existing solution config file
                    existingSettings = JsonHelpers.DeserializeFile<SolutionSettings>(solutionConfigFile);
                }
                catch
                {
                    var backupFile = $"{solutionConfigFile}.bak";
                    File.Move(solutionConfigFile, backupFile);
                }
            }

            // Next, let's get the properties for the classes for this config file
            var properties = GetPropertiesForClass(SolutionSettingsType, typeof(SolutionSettingFieldMetadata), 0, PropertySource.SolutionSettings, existingSettings);
            var output = ModifySolutionSettingProperties(templateName, outputDir, properties, solutionName);
            var finalOutput = output.OrderBy(x => x.Order).ToList();
            return (finalOutput, existingSettings != null);
        }

        /// <summary>
        /// Gets the preperation user settings.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <returns>Item 1: The properties we need user information on. Item 2: Whether an existing file was loaded</returns>
        public (List<Property> Properties, bool HadFile) GetPreperationUserSettings(string file)
        {
            // First, lets see if there is an existing template file
            Template? existingTemplate = null;
            if (File.Exists(file))
            {
                try
                {
                    existingTemplate = JsonHelpers.DeserializeFile<Template>(file);
                }
                catch
                {
                    // if it is an invalid template file, don't use it but turn it into a backup file
                    var backupFile = $"{file}.bak";
                    File.Move(file, backupFile);
                    existingTemplate = null;
                }
            }

            // Next, lets get the properties for the information and settings classes for this Templater
            var informationProperties = GetPropertiesForClass(TemplateInformationType, typeof(TemplateFieldMetadata), 0, PropertySource.TemplateInformation, existingTemplate?.Information);
            var settingsProperties = GetPropertiesForClass(TemplateSettingsType, typeof(TemplateFieldMetadata), informationProperties.Count, PropertySource.TemplateSettings, existingTemplate?.Settings);

            // Combine the properties, sort, and return
            var output = new List<Property>();
            output.AddRange(informationProperties);
            output.AddRange(settingsProperties);
            output = output.OrderBy(x => x.Order).ToList();
            return (output, existingTemplate != null);
        }

        /// <summary>
        /// Gets the solution settings for properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="configName">Name of the configuration.</param>
        /// <param name="preserveDefaultSolutionConfig">
        /// If flag is provided, the solution config will be preserved as the default value, if no specific config is set.
        /// </param>
        /// <returns>The solution settings instance for the properties specified.</returns>
        public (SolutionSettings, string) GetSolutionSettingsForProperties(List<Property> properties, string configName, bool preserveDefaultSolutionConfig)
        {
            IOHelpers.CreateDirectoryIfNotExists(Constants.TemplatesProjectConfigurationDirectory);
            var solutionSettings = PopulateNewInstanceWithProperties<SolutionSettings>(properties, SolutionSettingsType);

            var backupFile = string.Empty;
            // if the config name is the default one, back up the config
            if (configName == Constants.TemplatesProjectConfigurationFile)
            {
                var fileName = Path.GetFileNameWithoutExtension(configName);
                var extension = Path.GetExtension(configName);

                var backupFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                backupFile = Path.Combine(Constants.TemplatesProjectConfigurationDirectory, backupFileName);

                JsonHelpers.WriteObjectToFile(solutionSettings, configName);
                if (preserveDefaultSolutionConfig)
                {
                    File.Copy(configName, backupFile, true);
                }
                else
                {
                    File.Move(configName, backupFile);
                }
            }
            // otherwise, do nothing to the file...

            return (solutionSettings, backupFile);
        }

        /// <summary>
        /// Gets the template for properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="createTemplateFile">if set to <c>true</c> [create template file].</param>
        /// <returns>A template with the provided properties.</returns>
        public Template GetTemplateForProperties(List<Property> properties, string directory, bool createTemplateFile = true)
        {
            // Populate the child classes
            var informationProperties = properties.Where(x => x.SettingSource == PropertySource.TemplateInformation).ToList();
            var informationClass = PopulateNewInstanceWithProperties<TemplateInformation>(informationProperties, TemplateInformationType);

            var settingsProperties = properties.Where(x => x.SettingSource == PropertySource.TemplateSettings).ToList();
            var settingsClass = PopulateNewInstanceWithProperties<TemplateSettings>(settingsProperties, TemplateSettingsType);

            // Create the template class
            var template = new Template() { Information = informationClass, Settings = settingsClass };

            if (createTemplateFile)
            {
                // Create the template file
                var templateFile = Path.Combine(directory, Constants.TemplaterTemplatesInfoFileName);
                JsonHelpers.WriteObjectToFile(template, templateFile);
            }

            return template;
        }

        /// <summary>
        /// Prepares the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="log">The log.</param>
        /// <returns>The result of the preperation</returns>
        public abstract string PrepareTemplate(PrepareOptions options, Func<string, bool> log);

        /// <summary>
        /// Determines if the templater is valid for the template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>True if the templater is valid for the template, False otherwise.</returns>
        public bool TemplaterValidForTemplate(Template template)
        {
            var infoType = template.Information.GetType();
            var settingsType = template.Settings.GetType();

            var infoValid = infoType == TemplateInformationType;
            var settingsValid = settingsType == TemplateSettingsType;

            return infoValid && settingsValid;
        }

        /// <summary>
        /// Populates the new instance with properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties">The properties.</param>
        /// <param name="actualType">The actual type.</param>
        /// <returns>An instance populated with the provided properties.</returns>
        /// <exception cref="Exception">
        /// Unable to instantiate an instance of class {actualType} or Unable to find field {property.Name} in class {actualType}
        /// </exception>
        protected static T PopulateNewInstanceWithProperties<T>(List<Property> properties, Type actualType)
        {
            // create an instance and populate it with the properties
            var instance = (T)(Activator.CreateInstance(actualType) ?? throw new Exception($"Unable to instantiate an instance of class {actualType}"));
            foreach (var property in properties)
            {
                var field = actualType.GetField(property.Name);
                if (field == null)
                {
                    throw new Exception($"Unable to find field {property.Name} in class {actualType}");
                }

                field.SetValue(instance, property.CurrentValue);
            }

            return instance;
        }

        /// <summary>
        /// Gets the properties for class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attributeType">The attribute type.</param>
        /// <param name="orderOffset">The order offset.</param>
        /// <param name="source">The source of the properties.</param>
        /// <param name="existingInstance">The existing instance.</param>
        /// <returns>A list of propertied fields for the type provided.</returns>
        /// <exception cref="Exception">Field {field.Name} does not have a SettingMetadata Attribute!</exception>
        protected List<Property> GetPropertiesForClass(Type type, Type attributeType, int orderOffset, PropertySource source, object? existingInstance = null)
        {
            var output = new List<Property>();

            // Fetch the properties for the class
            var fields = type.GetFields().Where(x => x.CustomAttributes.Any(y => y.AttributeType == attributeType)).ToList();
            foreach (var field in fields)
            {
                // get the metadata attribute
                var metadata = (TemplateFieldMetadata?)field.GetCustomAttributes(attributeType, false)[0];
                if (metadata == null)
                {
                    throw new Exception($"Field {field.Name} does not have a SettingMetadata Attribute!");
                }

                // check if we have any allowed values
                List<string> allowedValues = [];
                var allowedValuesData = field.GetCustomAttributes(typeof(AllowedValue), false).Cast<AllowedValue>().ToList();
                if (allowedValuesData != null && allowedValuesData.Count > 0)
                {
                    foreach (var value in allowedValuesData)
                    {
                        List<string> temp;
                        switch (value.Type)
                        {
                            case PropertyType.Bool:
                            case PropertyType.Enum:
                                throw new Exception($"Unsupported allowed value type {value.Type}!");

                            case PropertyType.String:
                                allowedValues.Add(value.Value.ToString());
                                break;

                            case PropertyType.StringListComma:
                                temp = value.Value.ToString().Split(',').Select(x => x.Trim()).ToList();
                                allowedValues.AddRange(temp);
                                break;

                            case PropertyType.StringListSemiColan:
                                temp = value.Value.ToString().Split(';').Select(x => x.Trim()).ToList();
                                allowedValues.AddRange(temp);
                                break;

                            default:
                                throw new Exception($"Unknown or unsupported allowed value type {value.Type}!");
                        }
                    }
                }

                // determine the current value for the property and create it
                var currentValue = existingInstance != null ? field.GetValue(existingInstance) : null;
                if (currentValue == null && attributeType == typeof(SolutionSettingFieldMetadata))
                {
                    currentValue = ((SolutionSettingFieldMetadata)metadata).DefaultValue;
                }
                var propertyData = new Property()
                {
                    CurrentValue = currentValue,
                    DisplayName = metadata.DisplayName,
                    Name = field.Name,
                    Order = metadata.Order + orderOffset,
                    Type = metadata.Type,
                    SettingSource = source,
                    Metadata = metadata,
                    ActualType = field.FieldType,
                    AllowedValues = allowedValues,
                };
                output.Add(propertyData);
            }

            return output;
        }

        /// <summary>
        /// Modifies the solution setting properties.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="outputDir">The output directory.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="solutionName">The solution name.</param>
        /// <returns>The corrected solution setting properties.</returns>
        protected abstract List<Property> ModifySolutionSettingProperties(string templateName, string outputDir, List<Property> properties, string solutionName);

        /// <summary>
        /// Updates the simple replacement text during settings request.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="outputDir">The output dir.</param>
        /// <returns>The updated string.</returns>
        protected string UpdateSimpleReplacementTextDuringSettingsRequest(string value, string outputDir, string solutionName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            // TODO: Keep in Sync with Constants.SPECIAL_TEXT
            var replacers = new Dictionary<string, string>
            {
                { Constants.SPECIAL_TEXT[0], Environment.UserName },
                { Constants.SPECIAL_TEXT[1], Path.GetFileName(outputDir) },
                { Constants.SPECIAL_TEXT[2], solutionName }
            };

            foreach (var replacer in replacers)
            {
                value = value.Replace(replacer.Key, replacer.Value);
            }

            return value;
        }
    }
}
