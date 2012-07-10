using System;
using System.Collections.Generic;
using System.Reflection;

using Lucene.Net.Orm.Mapping.Configuration;


namespace Lucene.Net.Orm.Mapping
{
    public class DocumentMapping<TModel>
    {
        public ICollection<KeyValuePair<PropertyInfo, IFieldConfiguration>> PropertyMaps { get; set; }

        public ICollection<KeyValuePair<CustomMap<TModel>, IFieldConfiguration>> CustomMaps { get; set; }

        public ICollection<KeyValuePair<PropertyInfo, IReferenceConfiguration>> References { get; set; }


        public Func<TModel, float> Boost { get; set; }


        public DocumentMapping()
        {
            PropertyMaps = new List<KeyValuePair<PropertyInfo, IFieldConfiguration>>();
            CustomMaps = new List<KeyValuePair<CustomMap<TModel>, IFieldConfiguration>>();
            References = new List<KeyValuePair<PropertyInfo, IReferenceConfiguration>>();
        }
    }
}
