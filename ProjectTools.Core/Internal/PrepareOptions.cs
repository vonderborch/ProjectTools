using ProjectTools.Core.Internal.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTools.Core.Internal
{
    public class PrepareOptions
    {
        /// <summary>
        /// The directory of the project/solution being prepared as a template
        /// </summary>
        public string Directory;

        /// <summary>
        /// The output directory for the final template
        /// </summary>
        public string OutputDirectory;

        /// <summary>
        /// True to skip cleaning, False otherwise
        /// </summary>
        public bool SkipCleaning;

        /// <summary>
        /// Settings for the template
        /// </summary>
        public Template TemplateSettings;
    }
}
