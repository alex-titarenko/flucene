using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Lucene.Net.Orm.Mapping.Configuration;


namespace Lucene.Net.Orm.Mapping
{
    public delegate float Boosting<T>(T model);


    public abstract class DocumentMapBase<TModel>
    {
        public abstract IFieldConfiguration<TProperty> Map<TProperty>(Expression<Func<TModel, TProperty>> selector);

        public abstract IFieldConfiguration<TProperty> Map<TProperty>(Expression<Func<TModel, TProperty>> selector, string fieldName);

        public abstract IFieldConfiguration<TInput> CustomMap<TInput>(Func<TModel, TInput> selector, Action<TModel, IEnumerable<string>> setter, string fieldName);

        public abstract IFieldConfiguration<TInput> CustomField<TInput>(Func<TModel, TInput> selector, string fieldName);

        public abstract IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> CustomFields(Func<TModel, IEnumerable<KeyValuePair<string, object>>> selector);

        public abstract IReferenceConfiguration Reference<TProperty>(Expression<Func<TModel, TProperty>> selector);

        public abstract void Boost(Boosting<TModel> boost);
    }
}
