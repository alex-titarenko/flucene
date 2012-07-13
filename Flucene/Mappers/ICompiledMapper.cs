using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Documents;


namespace Lucene.Net.Orm.Mappers
{
    public interface ICompiledMapper<TModel>
    {
        Document GetDocument(TModel model);

        TModel GetModel(Document document);
    }
}
