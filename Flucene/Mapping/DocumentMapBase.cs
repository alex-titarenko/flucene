using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Mapping
{
    /// <summary>
    /// Returns the boost for the specified model.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    /// <param name="model">A model for which to determine the boost.</param>
    /// <returns>boost for model.</returns>
    public delegate float Boosting<in T>(T model);


    /// <summary>
    /// Represents the base class for all classes that implement the mechanism
    /// for the construction of mapping of model to lucene document using fluents interface.
    /// </summary>
    /// <typeparam name="TModel">The type of the model for which to build a mapping for lucene document.</typeparam>
    public abstract class DocumentMapBase<TModel>
    {
        /// <summary>
        /// Creates a property mapping and sets the field name equal to the property name.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to mapping.</typeparam>
        /// <param name="selector">A property selector to map.</param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        public abstract IFieldConfiguration<TProperty> Map<TProperty>(Expression<Func<TModel, TProperty>> selector);

        /// <summary>
        /// Creates a property mapping.
        /// </summary>
        /// <typeparam name="TProperty">The type of property to mapping.</typeparam>
        /// <param name="selector">A property selector to map.</param>
        /// <param name="fieldName">The string representing the name of the field.</param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        public abstract IFieldConfiguration<TProperty> Map<TProperty>(Expression<Func<TModel, TProperty>> selector, string fieldName);

        public abstract IFieldConfiguration<TInput> CustomMap<TInput>(Func<TModel, TInput> selector, Action<TModel, IEnumerable<string>> setter, string fieldName);

        public abstract IFieldConfiguration<TInput> CustomField<TInput>(Func<TModel, TInput> selector, string fieldName);

        public abstract IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> CustomFields(Func<TModel, IEnumerable<KeyValuePair<string, object>>> selector);

        public abstract IReferenceConfiguration Reference<TProperty>(Expression<Func<TModel, TProperty>> selector);

        public abstract void CustomAction(Action<TModel, Document> toDocument, Action<Document, TModel> toModel);

        public abstract void Boost(Boosting<TModel> boost);
    }
}
