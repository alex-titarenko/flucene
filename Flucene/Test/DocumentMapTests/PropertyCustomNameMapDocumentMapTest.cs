using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Orm.Test.DocumentMapTests
{
    [TestClass]
    public class PropertyCustomNameMapDocumentMapTest
        : DocumentMapTestBase<PropertyCustomNameMapDocumentMapTest.ModelMap, PropertyCustomNameMapDocumentMapTest.Model>
    {
        protected override void BuildExpectedModelMapping(DocumentMapping<Model> mapping)
        {
            mapping.Fields.Add(new FieldMapping("CustomName", new PropertyMember(GetProperty("Property"))));
        }


        public class Model
        {
            public string Property { get; set; }
        }

        public class ModelMap : DocumentMap<Model>
        {
            public ModelMap()
            {
                Map(x => x.Property, "CustomName");
            }
        }
    }
}
