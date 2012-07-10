using System;

using Lucene.Net.Documents;
using Lucene.Net.Orm.Mappers;


namespace Lucene.Net.Orm
{
    public interface IMappingsService
    {
        IDocumentMapper Mapper { get; set; }


        Document GetDocument<T>(T model) where T : new();

        T GetModel<T>(Document doc) where T : new();

        object GetModel(Document doc, Type modelType);
    }
}
