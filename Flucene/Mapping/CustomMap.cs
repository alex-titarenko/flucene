using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Orm.Mapping
{
    public class CustomMap<TModel>
    {
        public Func<TModel, object> Selector { get; set; }

        public Action<TModel, IEnumerable<string>> Setter { get; set; }
    }
}
