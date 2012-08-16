using System;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping;


namespace Lucene.Net.Odm.Mappers
{
    public interface IDocumentMapper
    {
        Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IMappingsService mappingService, string prefix = null);

        TModel GetModel<TModel>(DocumentMapping<TModel> mapping, Document document, IMappingsService mappingService, string prefix = null) where TModel : new();
    }
}
