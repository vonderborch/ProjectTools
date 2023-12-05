using ProjectTools.Core.Internal.Configuration;
using System.Text.Json.Serialization;

namespace ProjectTools.Core.Internal
{
    public class GenerateOptions
    {
        public bool CleanSolutionConfigFile;

        public string Directory;

        public bool OverrideExistingDirectory;

        public string SolutionConfigFile;

        public string SolutionName;

        public SolutionSettings SolutionSettings;

        public Template Template;

        [JsonIgnore]
        protected Dictionary<string, string> _replacementText = new();

        public string[][] GetSpecialReplacementText =>
            new string[][]
            {
                new string[] { "<CurrentUserName>", Environment.UserName },
                new string[] { "<ParentDir>", Path.GetFileName(Directory) },
                new string[] { "<SolutionName>", SolutionName },
            };

        public Dictionary<string, string> ReplacementsAndGuids
        {
            get
            {
                if (_replacementText.Count == 0)
                {
                    // add the guids...
                    var guidCount = Templater.Instance.TemplateGuidCounts[Template.Name];
                    for (var i = 1; i <= guidCount; i++)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var searchTerm = $"GUID{i.ToString(Constants.GUID_PADDING)}";
                        _replacementText.Add(searchTerm, guid);
                    }

                    foreach (var text in GetSpecialReplacementText)
                    {
                        _replacementText.Add(text[0], text[1]);
                    }

                    // add other replacements...
                    foreach (var replacement in Template.Settings.ReplacementText)
                    {
                        var searchTerm = replacement.Item1;
                        var replacementText = replacement.Item2;
                        for (var i = 0; i < GetSpecialReplacementText.Length; i++)
                        {
                            replacementText = replacementText.Replace(
                                GetSpecialReplacementText[i][0],
                                GetSpecialReplacementText[i][1]
                            );
                        }

                        _replacementText.Add(searchTerm, replacementText);
                    }
                }

                return _replacementText;
            }
        }

        public virtual void UpdateReplacementTextWithTags()
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
            _replacementText.Add(Constants.REGEX_TAGS[0], SolutionSettings.Author);
            _replacementText.Add(Constants.REGEX_TAGS[1], SolutionSettings.Company);
            _replacementText.Add(Constants.REGEX_TAGS[3], SolutionSettings.Description);
            _replacementText.Add(Constants.REGEX_TAGS[5], SolutionSettings.Version);
        }

        public string UpdateTextWithReplacements(string value)
        {
            foreach (var pair in ReplacementsAndGuids)
            {
                value = value.Replace(pair.Key, pair.Value);
            }

            return value;
        }
    }
}
