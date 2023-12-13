using ProjectTools.Core.Helpers;
using ProjectTools.Core.Templating;

namespace ProjectTools.Core.Implementations
{
    /// <summary>
    /// Metadata on a registered template
    /// </summary>
    internal class RegisteredTemplateMetadata
    {
        /// <summary>
        /// The registration data
        /// </summary>
        public readonly ImplementationRegister RegistrationData;

        /// <summary>
        /// The type
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredTemplateMetadata"/> class.
        /// </summary>
        /// <param name="registrationData">The registration data.</param>
        /// <param name="type">The type.</param>
        public RegisteredTemplateMetadata(ImplementationRegister registrationData, Type type)
        {
            RegistrationData = registrationData;
            Type = type;

            // Generate an instance of the templater
            var instance = (AbstractTemplater?)Activator.CreateInstance(Type);
            Instance = instance ?? throw new Exception($"Could not create instance of {Type.Name}");
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public AbstractTemplater Instance { get; }
    }
}
