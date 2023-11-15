using ProjectTools.Core.Internal.Implementations;
using ProjectTools.Core.Internal.Repositories;

namespace ProjectTools.Core.Internal
{
    public class Templater
    {
        private RepositoryCollection? _repositories;

        private Settings _settings;

        public Templater(Settings settings)
        {
            _settings = settings;
            AvailableTemplaters = TemplateFactory.TemplaterImplementations;
        }

        public List<AbstractTemplater> AvailableTemplaters { get; }

        public Dictionary<TemplaterImplementations, AbstractTemplater> AvailableTemplaterMap => AvailableTemplaters.ToDictionary(x => x.Implementation, x => x);

        public List<string> AvailableTemplaterShortNames
        {
            get
            {
                var templates = AvailableTemplaters.Select(x => x.ShortName).ToList();
                templates.Sort();
                return templates;
            }
        }

        public List<string> AvailableTemplaterLongNames
        {
            get
            {
                var templates = AvailableTemplaters.Select(x => x.LongName).ToList();
                templates.Sort();
                return templates;
            }
        }

        public RepositoryCollection Repositories
        {
            get
            {
                if (_repositories == null)
                {
                    _repositories = new(_settings.TemplateRepositories, true);
                }

                return _repositories;
            }
        }
        
        public string Prepare(PrepareOptions options, TemplaterImplementations implementation, Func<string, bool> log)
        {
            if (AvailableTemplaterMap.TryGetValue(implementation, out var templater))
            {
                return templater.Prepare(options, log);
            }

            throw new NotImplementedException($"A templater for type {implementation} is not implemented!");
        }
    }
}
