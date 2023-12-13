using ProjectTools.Core.Helpers;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Common;
using System.Diagnostics;

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
        /// Gets the preperation user settings.
        /// </summary>
        /// <returns>Item 1: The properties we need user information on. Item 2: Whether an existing file was loaded</returns>
        public (List<SettingProperty> Properties, bool HadFile) GetPreperationUserSettings(string file)
        {
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
            var informationProperties = GetPropertiesForClass(TemplateInformationType, 0, true, existingTemplate?.Information);
            var settingsProperties = GetPropertiesForClass(TemplateSettingsType, informationProperties.Count, false, existingTemplate?.Settings);

            // Combine the properties, sort, and return
            var output = new List<SettingProperty>();
            output.AddRange(informationProperties);
            output.AddRange(settingsProperties);
            output = output.OrderBy(x => x.Order).ToList();
            return (output, existingTemplate != null);
        }

        /// <summary>
        /// Gets the template for properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="createTemplateFile">if set to <c>true</c> [create template file].</param>
        /// <returns>A template with the provided properties.</returns>
        public Template GetTemplateForProperties(List<SettingProperty> properties, string directory, bool createTemplateFile = true)
        {
            // Populate the child classes
            var informationProperties = properties.Where(x => x.FromTemplateInformationClass).ToList();
            var informationClass = PopulateNewInstanceWithProperties<TemplateInformation>(informationProperties, TemplateInformationType);

            var settingsProperties = properties.Where(x => !x.FromTemplateInformationClass).ToList();
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
        /// Populates the new instance with properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties">The properties.</param>
        /// <param name="actualType">The actual type.</param>
        /// <returns>An instance populated with the provided properties.</returns>
        /// <exception cref="Exception">
        /// Unable to instantiate an instance of class {actualType} or Unable to find field {property.Name} in class {actualType}
        /// </exception>
        protected static T PopulateNewInstanceWithProperties<T>(List<SettingProperty> properties, Type actualType)
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
        /// <param name="orderOffset">The order offset.</param>
        /// <param name="isTemplateInformationClass">if set to <c>true</c> [is template information class].</param>
        /// <param name="existingInstance">The existing instance.</param>
        /// <returns>A list of propertied fields for the type provided.</returns>
        /// <exception cref="Exception">Field {field.Name} does not have a SettingMetadata Attribute!</exception>
        protected List<SettingProperty> GetPropertiesForClass(Type type, int orderOffset, bool isTemplateInformationClass, object? existingInstance = null)
        {
            var output = new List<SettingProperty>();

            // Fetch the properties for the class
            var fields = type.GetFields().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(SettingMetadata))).ToList();
            foreach (var field in fields)
            {
                var metadata = (SettingMetadata?)field.GetCustomAttributes(typeof(SettingMetadata), false)[0];
                if (metadata == null)
                {
                    throw new Exception($"Field {field.Name} does not have a SettingMetadata Attribute!");
                }

                var currentValue = existingInstance != null ? field.GetValue(existingInstance) : null;
                var propertyData = new SettingProperty()
                {
                    CurrentValue = currentValue,
                    DisplayName = metadata.DisplayName,
                    Name = field.Name,
                    Order = metadata.Order + orderOffset,
                    Type = metadata.Type,
                    FromTemplateInformationClass = isTemplateInformationClass
                };
                output.Add(propertyData);
            }

            return output;
        }
    }
}
