using System;


namespace Lucene.Net.Odm.Mapping
{
    public interface IMappingProvider<TModel>
    {
        DocumentMapping<TModel> GetMapping();
    }
}
