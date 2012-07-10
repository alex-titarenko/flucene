using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Orm.Helpers;
using Lucene.Net.Orm.Mapping.Configuration;


namespace Lucene.Net.Orm.Mapping
{
    public class DocumentMap<TModel> : DocumentMapBase<TModel>, IMappingProvider<TModel> where TModel : new()
    {
        protected IMappingConfigurationFactory MappingConfigurationFactory { get; set; }

        protected DocumentMapping<TModel> Mapping { get; set; }


        public DocumentMap()
        {
            Mapping = new DocumentMapping<TModel>();
            MappingConfigurationFactory = new StandardMappingConfigurationFactory();
        }


        public override IFieldConfiguration Map(Expression<Func<TModel, object>> selector)
        {
            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            return Map(selector, property.Name);
        }

        public override IFieldConfiguration Map(Expression<Func<TModel, object>> selector, string fieldName)
        {
            IFieldConfiguration field = MappingConfigurationFactory.CreateFieldConfiguration(fieldName);
            
            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            Mapping.PropertyMaps.Add(new KeyValuePair<PropertyInfo, IFieldConfiguration>(property, field));

            return field;
        }

        public override IFieldConfiguration CustomMap(Func<TModel, object> selector, Action<TModel, IEnumerable<string>> setter, string fieldName)
        {
            IFieldConfiguration field = MappingConfigurationFactory.CreateFieldConfiguration(fieldName);
            CustomMap<TModel> customMap = new CustomMap<TModel>(selector, setter);

            Mapping.CustomMaps.Add(new KeyValuePair<CustomMap<TModel>, IFieldConfiguration>(customMap, field));
            return field;
        }

        public override IFieldConfiguration CustomField(Func<TModel, object> selector, string fieldName)
        {
            return CustomMap(selector, null, fieldName);
        }

        public override void CustomFields(Func<TModel, IEnumerable<KeyValuePair<string, object>>> selector)
        {
        }

        public override IReferenceConfiguration Reference(Expression<Func<TModel, object>> selector)
        {
            IReferenceConfiguration reference = MappingConfigurationFactory.CreateReferenceConfiguration();

            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            Mapping.ReferenceMaps.Add(new KeyValuePair<PropertyInfo, IReferenceConfiguration>(property, reference));
            
            return reference;
        }

        public override void SetBoost(Func<TModel, float> documentBoost)
        {
            Mapping.Boost = documentBoost;
        }


        #region IMappingProvider<TModel> Members

        public DocumentMapping<TModel> GetMapping()
        {
            return Mapping;
        }

        #endregion
    }
}
