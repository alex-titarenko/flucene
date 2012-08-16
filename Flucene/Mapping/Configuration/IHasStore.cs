using System;
using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public interface IHasStore
    {
        Field.Store Store { get; set; }
    }
}
