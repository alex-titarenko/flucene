using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Helpers;
using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Mapping
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


        public override IFieldConfiguration<TProperty> Map<TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            return Map(selector, property.Name);
        }

        public override IFieldConfiguration<TProperty> Map<TProperty>(Expression<Func<TModel, TProperty>> selector, string fieldName)
        {
            IFieldConfiguration<TProperty> field = MappingConfigurationFactory.CreateFieldConfiguration<TProperty>(fieldName);
            
            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            Mapping.PropertyMappings.Add(new PropertyMapping(property, field));

            return field;
        }

        public override IFieldConfiguration<TInput> CustomMap<TInput>(Func<TModel, TInput> selector, Action<TModel, IEnumerable<string>> setter, string fieldName)
        {
            IFieldConfiguration<TInput> field = MappingConfigurationFactory.CreateFieldConfiguration<TInput>(fieldName);
            CustomMapping<TModel> customMapping = new CustomMapping<TModel>()
            {
                Selector = (x) => selector(x),
                Setter = setter,
                FieldConfiguration = field
            };
            
            Mapping.CustomMappings.Add(customMapping);
            return field;
        }

        public override IFieldConfiguration<TInput> CustomField<TInput>(Func<TModel, TInput> selector, string fieldName)
        {
            return CustomMap(selector, null, fieldName);
        }

        public override IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> CustomFields(Func<TModel, IEnumerable<KeyValuePair<string, object>>> selector)
        {
            IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> fields =
                MappingConfigurationFactory.CreateFieldsConfiguration();
            CustomMapping<TModel> customMapping = new CustomMapping<TModel>(selector, null) { FieldConfiguration = fields };

            Mapping.CustomMappings.Add(customMapping);
            return fields;
        }

        public override IReferenceConfiguration Reference<TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            IReferenceConfiguration reference = MappingConfigurationFactory.CreateReferenceConfiguration();

            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            Mapping.ReferenceMappings.Add(new KeyValuePair<PropertyInfo, IReferenceConfiguration>(property, reference));
            
            return reference;
        }

        public override void CustomAction(Action<TModel, Document> toDocument, Action<Document, TModel> toModel)
        {
            Mapping.CustomActions.Add(new CustomAction<TModel>(toDocument, toModel));
        }

        public override void Boost(Boosting<TModel> documentBoost)
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
