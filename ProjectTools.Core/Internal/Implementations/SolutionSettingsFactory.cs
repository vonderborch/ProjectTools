using ProjectTools.Core.Internal.Configuration;
using ProjectTools.Core.Internal.Implementations.DotSln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTools.Core.Internal.Implementations
{
    public static class SolutionSettingsFactory
    {
        public SolutionSettings GetSettingsForImplementation(TemplaterImplementations implementation)
        {
            switch (implementation)
            {
                case TemplaterImplementations.DotSln:
                    return new DotSlnSolutionSettings();
                default:
                    return new SolutionSettings();
            }
        }

        public List<> GetFieldsAndTypesForImplementation(TemplaterImplementations implementation)
        {
            PropertyInfo[] properties;
            switch (implementation)
            {
                case TemplaterImplementations.DotSln:
                    properties = typeof(DotSlnSolutionSettings).GetProperties();
                    break;
                default:
                    properties = typeof(SolutionSettings).GetProperties();
                    break;
            }
        }
    }
}
