using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTools.Core.Internal.Implementations.DotSln
{
    public class DotSlnGenerateOptions : GenerateOptions
    {
        public override void UpdateReplacementTextWithTags()
        {
            /* NOTE: Keep in sync with Constants.REGEX_TAGS
             * 0 = Author
             * 1 = Company
             * 2 = Tags
             * 3 = Description
             * 4 = License
             * 5 = Version
             */
            if (_replacementText.Count == 0)
            {
                _ = ReplacementsAndGuids;
            }
            var settings = (DotSlnSolutionSettings)SolutionSettings;

            _replacementText.Add(Constants.REGEX_TAGS[0], settings.Author);
            _replacementText.Add(Constants.REGEX_TAGS[1], settings.Company);
            _replacementText.Add(Constants.REGEX_TAGS[2], string.Join(",", settings.Tags));
            _replacementText.Add(Constants.REGEX_TAGS[3], settings.Description);
            _replacementText.Add(Constants.REGEX_TAGS[4], settings.LicenseExpression);
            _replacementText.Add(Constants.REGEX_TAGS[5], settings.Version);
        }
    }
}
