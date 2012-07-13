using System;
using System.Collections.Generic;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public interface IMappingConfigurationFactory
    {
        IFieldConfiguration CreateFieldConfiguration(string fieldName, Type fieldType);

        IFieldConfiguration<TInput> CreateFieldConfiguration<TInput>(string fieldName);

        IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> CreateFieldsConfiguration();

        IReferenceConfiguration CreateReferenceConfiguration();
    }
}
