using System;
using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public interface IHasIndex
    {
        Field.Index Index { get; set; }
    }
}
