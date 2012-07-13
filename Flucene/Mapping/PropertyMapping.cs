using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using Lucene.Net.Odm.Mapping.Configuration;


namespace Lucene.Net.Odm.Mapping
{
    public class PropertyMapping
    {
        public PropertyInfo PropertyInfo { get; set; }

        public IFieldConfiguration FieldConfiguration { get; set; }


        public PropertyMapping(PropertyInfo property, IFieldConfiguration field)
        {
            PropertyInfo = property;
            FieldConfiguration = field;
        }
    }
}
