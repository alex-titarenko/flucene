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


        public CustomMap()
        {
        }

        public CustomMap(Func<TModel, object> selector)
            : this(selector, null)
        {
        }

        public CustomMap(Func<TModel, object> selector, Action<TModel, IEnumerable<string>> setter)
        {
            Selector = selector;
            Setter = setter;
        }
    }
}
