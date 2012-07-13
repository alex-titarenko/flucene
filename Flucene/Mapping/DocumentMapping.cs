using System;
using System.Collections.Generic;
using System.Reflection;

using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Mapping
{
    public class DocumentMapping<TModel>
    {
        public ICollection<PropertyMapping> PropertyMappings { get; set; }

        public ICollection<CustomMapping<TModel>> CustomMappings { get; set; }

        public ICollection<KeyValuePair<PropertyInfo, IReferenceConfiguration>> ReferenceMappings { get; set; }

        public ICollection<CustomAction<TModel>> CustomActions { get; set; }

        public Boosting<TModel> Boost { get; set; }


        public DocumentMapping()
        {
            PropertyMappings = new List<PropertyMapping>();
            CustomMappings = new List<CustomMapping<TModel>>();
            ReferenceMappings = new List<KeyValuePair<PropertyInfo, IReferenceConfiguration>>();
            CustomActions = new List<CustomAction<TModel>>();
        }
    }
}
