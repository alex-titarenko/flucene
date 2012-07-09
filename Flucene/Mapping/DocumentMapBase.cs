using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Lucene.Net.Documents;


namespace Lucene.Net.Orm.Mapping
{
    public abstract class DocumentMapBase<TModel>
    {
        public abstract IFieldConfiguration Map(Expression<Func<TModel, object>> selector);

        public abstract IFieldConfiguration Map(Expression<Func<TModel, object>> selector, string fieldName);

        public abstract IFieldConfiguration CustomMap(Func<TModel, object> selector, Action<TModel, IEnumerable<string>> setter, string fieldName);

        public abstract IFieldConfiguration CustomField(Func<TModel, object> selector, string fieldName);

        public abstract void CustomFields(Func<TModel, IEnumerable<KeyValuePair<string, object>>> selector);

        public abstract IReferenceConfiguration Reference(Expression<Func<TModel, object>> selector);

        public abstract void SetBoost(Func<TModel, float> boost);
    }
}
