using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

using Lucene.Net.Documents;

using Lucene.Net.Orm.Helpers;


namespace Lucene.Net.Orm.Mapping
{
    public class DocumentMap<TModel> : DocumentMapBase<TModel>, IMappingProvider<TModel> where TModel : new()
    {
        protected DocumentMapping<TModel> Mapping { get; set; }


        public DocumentMap()
        {
            Mapping = new DocumentMapping<TModel>();
        }


        public override IFieldConfiguration Map(Expression<Func<TModel, object>> selector)
        {
            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            return Map(selector, property.Name);
        }

        public override IFieldConfiguration Map(Expression<Func<TModel, object>> selector, string fieldName)
        {
            IFieldConfiguration field = new FieldConfiguration(fieldName);
            
            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            Mapping.MapFields.Add(new KeyValuePair<PropertyInfo, IFieldConfiguration>(property, field));

            return field;
        }

        public override IFieldConfiguration CustomMap(Func<TModel, object> selector, Action<TModel, IEnumerable<string>> setter, string fieldName)
        {
            return null;
        }

        public override IFieldConfiguration CustomField(Func<TModel, object> selector, string fieldName)
        {
            IFieldConfiguration field = new FieldConfiguration(fieldName);
            Mapping.CustomFields.Add(new KeyValuePair<Func<TModel, object>, IFieldConfiguration>(selector, field));
            
            return field;
        }

        public override void CustomFields(Func<TModel, IEnumerable<KeyValuePair<string, object>>> selector)
        {
        }

        public override IReferenceConfiguration Reference(Expression<Func<TModel, object>> selector)
        {
            IReferenceConfiguration reference = new ReferenceConfiguration();

            PropertyInfo property = ExpressionHelper.ExtractPropertyInfo(selector);
            Mapping.References.Add(new KeyValuePair<PropertyInfo, IReferenceConfiguration>(property, reference));
            
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
