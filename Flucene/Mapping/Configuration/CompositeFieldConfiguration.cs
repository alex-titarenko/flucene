using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Mapping.Configuration
{

    public class CompositeFieldConfiguration : IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>>, IFieldConfiguration
    {
        private Boosting<IEnumerable<KeyValuePair<string, object>>> _boost;

        private List<Action<IFieldConfiguration>> _actions =
            new List<Action<IFieldConfiguration>>();


        public IMappingConfigurationFactory ConfigurationFactory { get; private set; }


        public CompositeFieldConfiguration(IMappingConfigurationFactory factory)
        {
            ConfigurationFactory = factory;
        }


        #region IFieldConfiguration Members

        public string FieldName
        {
            get
            {
                return null;
            }
        }


        IFieldConfiguration IFieldConfiguration.Analyze()
        {
            return Analyze();
        }

        IFieldConfiguration IFieldConfiguration.NoNormsAnalyze()
        {
            return NoNormsAnalyze();
        }

        IFieldConfiguration IFieldConfiguration.NotIndex()
        {
            return NotIndex();
        }

        IFieldConfiguration IFieldConfiguration.NotAnalyze()
        {
            return NotAnalyze();
        }

        IFieldConfiguration IFieldConfiguration.NoNormsNotAnalyze()
        {
            return NoNormsNotAnalyze();
        }


        IFieldConfiguration IFieldConfiguration.Store()
        {
            return Store();
        }

        IFieldConfiguration IFieldConfiguration.Compress()
        {
            return Compress();
        }

        IFieldConfiguration IFieldConfiguration.NotStore()
        {
            return NotStore();
        }


        IFieldConfiguration IFieldConfiguration.Boost(Boosting<Object> boost)
        {
            return Boost(((IEnumerable<KeyValuePair<string, object>> input) => boost(input)));
        }


        public IEnumerable<Fieldable> GetFields(object value)
        {
            var pairs = value as IEnumerable<KeyValuePair<string, object>>;

            List<Fieldable> fields = new List<Fieldable>();

            if (pairs != null)
            {
                foreach (KeyValuePair<string, object> pair in pairs)
                {
                    IFieldConfiguration configuration = ConfigurationFactory.CreateFieldConfiguration(pair.Key, pair.Value.GetType());
                    _actions.ForEach(action => action(configuration));
                    configuration.Boost(x => _boost(pairs));

                    fields.AddRange(configuration.GetFields(pair.Value));
                }
            }
            else
            {
                throw new ArgumentException(
                    "For composite field configuration need to 'IEnumerable<KeyValuePair<string, object>>' type of argument.");
            }

            return fields;
        }

        #endregion

        #region IFieldConfiguration<TInput> Members

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> Analyze()
        {
            _actions.Add((x) => x.Analyze());
            return this;
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> NoNormsAnalyze()
        {
            _actions.Add((x) => x.NoNormsAnalyze());
            return this;
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> NotIndex()
        {
            _actions.Add((x) => x.NotIndex());
            return this;
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> NotAnalyze()
        {
            _actions.Add((x) => x.NotAnalyze());
            return this;
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> NoNormsNotAnalyze()
        {
            _actions.Add((x) => x.NoNormsNotAnalyze());
            return this;
        }


        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> Store()
        {
            _actions.Add((x) => x.Store());
            return this;
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> Compress()
        {
            _actions.Add((x) => x.Compress());
            return this;
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> NotStore()
        {
            _actions.Add((x) => x.NotStore());
            return this;
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> Boost(Boosting<IEnumerable<KeyValuePair<string, object>>> boost)
        {
            _boost = boost;
            return this;
        }

        #endregion
    }
}
