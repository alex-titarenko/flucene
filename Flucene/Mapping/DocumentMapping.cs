using System;
using System.Collections.Generic;
using System.Reflection;

using Lucene.Net.Orm.Mapping.Configuration;


namespace Lucene.Net.Orm.Mapping
{
    public class DocumentMapping<TModel>
    {
        public ICollection<PropertyMapping> PropertyMappings { get; set; }

        public ICollection<CustomMapping<TModel>> CustomMappings { get; set; }

        public ICollection<KeyValuePair<PropertyInfo, IReferenceConfiguration>> ReferenceMaps { get; set; }

        public Boosting<TModel> Boost { get; set; }


        public DocumentMapping()
        {
            PropertyMappings = new List<PropertyMapping>();
            CustomMappings = new List<CustomMapping<TModel>>();
            ReferenceMaps = new List<KeyValuePair<PropertyInfo, IReferenceConfiguration>>();
        }
    }
}
