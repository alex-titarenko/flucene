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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput">The type of object to mapping.</typeparam>
        /// <param name="selector">A selector function for mapping model to document.</param>
        /// <param name="setter">An action that performed at the moment of the mapping document to model.</param>
        /// <param name="fieldName">The string representing the name of the field.</param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        public abstract IFieldConfiguration<TInput> CustomMap<TInput>(Func<TModel, TInput> selector, Action<TModel, IEnumerable<string>> setter, string fieldName);

        /// <summary>
        /// Creates the custom field.
        /// </summary>
        /// <typeparam name="TInput">The type of object to mapping.</typeparam>
        /// <param name="selector"></param>
        /// <param name="fieldName">The string representing the name of the field.</param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        public abstract IFieldConfiguration<TInput> CustomField<TInput>(Func<TModel, TInput> selector, string fieldName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        public abstract IFieldConfiguration<IEnumerable<KeyValuePair<string, object>>> CustomFields(Func<TModel, IEnumerable<KeyValuePair<string, object>>> selector);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public abstract IReferenceConfiguration Reference<TProperty>(Expression<Func<TModel, TProperty>> selector);

        /// <summary>
        /// Adds a custom actions that performed for forward and reverse mapping.
        /// </summary>
        /// <param name="toDocument">Action that performed at the moment of the mapping model to document.</param>
        /// <param name="toModel">Action that performed at the moment of the mapping document to model.</param>
        public abstract void CustomAction(Action<TModel, Document> toDocument, Action<Document, TModel> toModel);

        /// <summary>
        /// Sets the boost function for the target document.
        /// </summary>
        /// <param name="boost">A boost function for the target document.</param>
        public abstract void Boost(Boosting<TModel> boost);
    }
}
