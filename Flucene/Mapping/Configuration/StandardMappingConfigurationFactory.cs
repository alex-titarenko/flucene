using System;
using System.Linq;
using System.Collections.Generic;


namespace Lucene.Net.Orm.Mapping.Configuration
{
    public class StandardMappingConfigurationFactory : IMappingConfigurationFactory
    {
        #region IMappingConfigurationFactory Members

        public IFieldConfiguration CreateFieldConfiguration(string fieldName, Type fieldType)
        {
            return (IFieldConfiguration)Activator.CreateInstance((typeof(FieldConfiguration<>).MakeGenericType(fieldType)), fieldName);
        }

        public IFieldConfiguration<TInput> CreateFieldConfiguration<TInput>(string fieldName)
        {
            return new FieldConfiguration<TInput>(fieldName);
        }

        public IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> CreateFieldsConfiguration()
        {
            return new CompositeFieldConfiguration(this);
        }

        public IReferenceConfiguration CreateReferenceConfiguration()
        {
            return new ReferenceConfiguration();
        }

        #endregion
    }
}
