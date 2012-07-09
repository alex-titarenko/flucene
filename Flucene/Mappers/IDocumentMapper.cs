using System;

using Lucene.Net.Documents;
using Lucene.Net.Orm.Mapping;


namespace Lucene.Net.Orm.Mappers
{
    public interface IDocumentMapper
    {
        Document GetDocument<TModel>(DocumentMapping<TModel> mapping, TModel model, IFluentMappingsService mappingService);

        TModel GetModel<TModel>(DocumentMapping<TModel> mapping, Document document, IFluentMappingsService mappingService) where TModel : new();
    }
}
