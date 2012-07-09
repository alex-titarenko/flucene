using System;


namespace Lucene.Net.Orm.Mapping
{
    public interface IMappingProvider<TModel>
    {
        DocumentMapping<TModel> GetMapping();
    }
}
