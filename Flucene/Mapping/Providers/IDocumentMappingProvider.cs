using System;


namespace Lucene.Net.Odm.Mapping.Providers
{
    public interface IDocumentMappingProvider<TModel> : IMappingProvider<DocumentMapping<TModel>>
    {
    }
}
