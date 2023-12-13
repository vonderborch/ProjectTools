using ProjectTools.Core.Implementations;

namespace ProjectTools.Core.Helpers
{
    /// <summary>
    /// An attribute handling registration of an implementation.
    /// </summary>
    /// <seealso cref="System.Attribute"/>
    [AttributeUsage(AttributeTargets.Class)]
    public class ImplementationRegister : Attribute
    {
        /// <summary>
        /// The description
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// The name
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// The implementation
        /// </summary>
        public readonly Implementation Implementation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementationRegister"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="description">The description.</param>
        public ImplementationRegister(string displayName, Implementation implementation, string description)
        {
            DisplayName = displayName;
            Implementation = implementation;
            Description = description;
        }
    }
}
