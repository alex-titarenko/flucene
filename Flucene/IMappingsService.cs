using System;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mappers;


namespace Lucene.Net.Odm
{
    public interface IMappingsService
    {
        Document GetDocument<T>(T model, string prefix = null) where T : new();

        T GetModel<T>(Document doc) where T : new();

        object GetModel(Document doc, Type modelType, string prefix = null);
    }
}
