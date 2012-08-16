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
        /// <typeparam name="TMember">The type of property to mapping.</typeparam>
        /// <param name="expression">A property selector to map.</param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        protected abstract FieldConfiguration Map<TMember>(Expression<Func<TModel, TMember>> expression);

        /// <summary>
        /// Creates a property mapping.
        /// </summary>
        /// <typeparam name="TMember">The type of property to mapping.</typeparam>
        /// <param name="expression">A property selector to map.</param>
        /// <param name="fieldName">The string representing the name of the field.</param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        protected abstract FieldConfiguration Map<TMember>(Expression<Func<TModel, TMember>> expression, string fieldName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getter">A selector function for mapping model to document.</param>
        /// <param name="setter">An action that performed at the moment of the mapping document to model.</param>
        /// <param name="fieldName">The string representing the name of the field.</param>
        /// <returns>fluent interface to configure the field to mapping.</returns>
        protected abstract FieldConfiguration Map<TInput>(Func<TModel, TInput> getter, Action<TModel, IEnumerable<string>> setter, string fieldName);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEmbedded"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        protected abstract EmbeddedConfiguration Embedded<TEmbedded>(Expression<Func<TModel, TEmbedded>> property);

        protected abstract EmbeddedCollectionConfiguration<TChild> HasMany<TChild>(Expression<Func<TModel, IEnumerable<TChild>>> property);

        /// <summary>
        /// Adds a custom actions that performed for forward and reverse mapping.
        /// </summary>
        /// <param name="toDocument">Action that performed at the moment of the mapping model to document.</param>
        /// <param name="toModel">Action that performed at the moment of the mapping document to model.</param>
        protected abstract void Custom(Action<TModel, Document> toDocument, Action<Document, TModel> toModel);

        /// <summary>
        /// Sets the boost function for the target document.
        /// </summary>
        /// <param name="boost">A boost function for the target document.</param>
        protected abstract void Boost(Boosting<TModel> boost);
    }
}
