using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Orm.Mapping
{
    public interface IMappingProvider<TModel>
    {
        DocumentMapping<TModel> GetMapping();
    }
}
