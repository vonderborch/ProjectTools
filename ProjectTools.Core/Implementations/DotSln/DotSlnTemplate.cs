using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Implementations.DotSln
{
    /// <summary>
    /// A template representing a .sln project/solution
    /// </summary>
    /// <seealso cref="Templating.Common.AbstractTemplate" />
    public class DotSlnTemplate : AbstractTemplate
    {
        /// <summary>
        /// The unique identifier count
        /// </summary>
        public int GuidCount = -1;
    }
}
