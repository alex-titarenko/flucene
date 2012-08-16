using System;


namespace Lucene.Net.Odm.Mapping.Providers
{
    public interface IMappingProvider<T>
    {
        T GetMapping();
    }
}
