using System;
using System.Collections.Generic;
using System.Reflection;

using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Mapping
{
    public class DocumentMapping<TModel>
    {
        public ICollection<FieldMapping> Fields { get; set; }

        public ICollection<EmbeddedMapping> Embedded { get; set; }

        public ICollection<CustomAction<TModel>> CustomActions { get; set; }

        public Boosting<TModel> Boost { get; set; }


        public DocumentMapping()
        {
            Fields = new List<FieldMapping>();
            Embedded = new List<EmbeddedMapping>();
            CustomActions = new List<CustomAction<TModel>>();
        }
    }
}
