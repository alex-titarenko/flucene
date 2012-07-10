using System;


namespace Lucene.Net.Orm.Mapping.Configuration
{
    public interface IMappingConfigurationFactory
    {
        IFieldConfiguration CreateFieldConfiguration(string fieldName);

        IReferenceConfiguration CreateReferenceConfiguration();
    }
}
