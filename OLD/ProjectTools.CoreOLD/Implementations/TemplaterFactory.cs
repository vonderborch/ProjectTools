using System.Reflection;
using ProjectTools.CoreOLD.Helpers;
using ProjectTools.CoreOLD.Templating;

namespace ProjectTools.CoreOLD.Implementations
{
    /// <summary>
    /// A factory for returning the correct templater.
    /// </summary>
    public static class TemplaterFactory
    {
        /// <summary>
        /// The templaters
        /// </summary>
        private static readonly Dictionary<Implementation, RegisteredTemplateMetadata> _templaters;

        /// <summary>
        /// Initializes the <see cref="TemplaterFactory"/> class.
        /// </summary>
        static TemplaterFactory()
        {
            // Load all templaters from the calling assembly and see if they should be registered.
            var typesInAssembly = Assembly.GetCallingAssembly().GetTypes();
            var templaterType = typeof(AbstractTemplater);
            var implementations = typesInAssembly.Where(x => x.IsClass && !x.IsAbstract && x.BaseType == templaterType && x.IsDefined(typeof(ImplementationRegister), false)).ToList();

            _templaters = [];
            foreach (var implementation in implementations)
            {
                var registrationAttribute = (ImplementationRegister)implementation.GetCustomAttribute(typeof(ImplementationRegister), false);

                _templaters.Add(registrationAttribute.Implementation, new RegisteredTemplateMetadata(registrationAttribute, implementation));
            }
        }

        /// <summary>
        /// Gets the implementations.
        /// </summary>
        /// <returns>A list of registered implementations.</returns>
        public static List<Implementation> GetImplementations()
        {
            return _templaters.Keys.ToList();
        }

        /// <summary>
        /// Gets the templater for a specified implementation.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns>The templater for the implementation.</returns>
        public static AbstractTemplater GetTemplater(Implementation implementation)
        {
            return _templaters.TryGetValue(implementation, out var templater)
                ? templater.Instance
                : throw new ArgumentException($"No templater found for implementation {implementation}");
        }

        /// <summary>
        /// Gets all registered templaters.
        /// </summary>
        /// <returns>A list of all registered templaters.</returns>
        internal static List<AbstractTemplater> GetAllRegisteredTemplaters()
        {
            var implementations = GetImplementations();
            var output = new List<AbstractTemplater>();

            foreach (var implementation in implementations)
            {
                output.Add(GetTemplater(implementation));
            }

            return output;
        }
    }
}
