using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Lucene.Net.Orm.Mapping
{
    public class DocumentMapping<TModel>
    {
        public ICollection<KeyValuePair<PropertyInfo, IFieldConfiguration>> MapFields { get; set; }

        public ICollection<KeyValuePair<Func<TModel, Object>, IFieldConfiguration>> CustomFields { get; set; }

        public ICollection<KeyValuePair<CustomMap<TModel>, IFieldConfiguration>> CustomMaps { get; set; }

        public ICollection<KeyValuePair<PropertyInfo, IReferenceConfiguration>> References { get; set; }


        public Func<TModel, float> Boost { get; set; }


        public DocumentMapping()
        {
            MapFields = new List<KeyValuePair<PropertyInfo, IFieldConfiguration>>();
            CustomFields = new List<KeyValuePair<Func<TModel, Object>, IFieldConfiguration>>();
            References = new List<KeyValuePair<PropertyInfo, IReferenceConfiguration>>();
        }
    }
}
