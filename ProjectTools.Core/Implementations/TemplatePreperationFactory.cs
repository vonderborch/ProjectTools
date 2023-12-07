using ProjectTools.Core.Implementations.DotSln;
using ProjectTools.Core.Templating.Common;
using ProjectTools.Core.Templating.Preparation;

namespace ProjectTools.Core.Implementations
{
    /// <summary>
    /// A factory for getting the correct template preparer.
    /// </summary>
    public static class TemplatePreperationFactory
    {
        public static AbstractTemplate GetTemplate(Implementation implementation)
        {
            return implementation switch
            {
                Implementation.DotSln => new DotSlnTemplate(),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Gets the template preparer.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <returns>A template preparer for the specified implementation.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static AbstractTemplatePreparer GetTemplatePreparer(Implementation implementation)
        {
            return implementation switch
            {
                Implementation.DotSln => new DotSlnTemplatePreparer(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
