using System;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping;


namespace Lucene.Net.Odm.Mappers
{
    /// <summary>
    /// Represents the document mapper for two-way model-document conversion.
    /// </summary>
    public interface IDocumentMapper
    {
        /// <summary>
        /// Converts the specified model to equivalent document using the specified document mapping.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mapping">A document mapping for conversion.</param>
        /// <param name="model">A model for conversion.</param>
        /// <param name="mappingService">A mapping service.</param>
        /// <param name="prefix">A <see cref="System.String"/> represents field prefix.</param>
        /// <returns>document converted from the model.</returns>
        Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IMappingsService mappingService, string prefix = null);

        /// <summary>
        /// Converts the specified document to model using the specified document mapping.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mapping">A document mapping for conversion.</param>
        /// <param name="document">A document for conversion.</param>
        /// <param name="mappingService">A mapping service.</param>
        /// <param name="prefix">A <see cref="System.String"/> represents field prefix.</param>
        /// <returns>model converted from the document.</returns>
        TModel GetModel<TModel>(DocumentMapping<TModel> mapping, Document document, IMappingsService mappingService, string prefix = null) where TModel : new();
    }
}
