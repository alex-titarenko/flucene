using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Odm.Mapping.Configuration;

namespace Lucene.Net.Odm.Mapping
{
    public class CustomMapping<TModel>
    {
        public Func<TModel, object> Selector { get; set; }

        public Action<TModel, IEnumerable<String>> Setter { get; set; }

        public IFieldConfiguration FieldConfiguration { get; set; }


        public CustomMapping()
        {
        }

        public CustomMapping(Func<TModel, object> selector)
            : this(selector, null)
        {
        }

        public CustomMapping(Func<TModel, object> selector, Action<TModel, IEnumerable<string>> setter)
        {
            Selector = selector;
            Setter = setter;
        }
    }
}
