using System;


namespace Lucene.Net.Orm.Mapping.Configuration
{
    public class StandardMappingConfigurationFactory : IMappingConfigurationFactory
    {
        #region IMappingConfigurationFactory Members

        public IFieldConfiguration CreateFieldConfiguration(string fieldName)
        {
            return new FieldConfiguration(fieldName);
        }

        public IReferenceConfiguration CreateReferenceConfiguration()
        {
            return new ReferenceConfiguration();
        }

        #endregion
    }
}
