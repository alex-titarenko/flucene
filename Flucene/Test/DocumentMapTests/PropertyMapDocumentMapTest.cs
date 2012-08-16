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
    public class PropertyMapDocumentMapTest
        : DocumentMapTestBase<PropertyMapDocumentMapTest.ModelMap, PropertyMapDocumentMapTest.Model>
    {
        protected override void BuildExpectedModelMapping(DocumentMapping<Model> mapping)
        {
            mapping.Fields.Add(new FieldMapping("Field", new PropertyMember(GetProperty("Field"))));
        }

        public class Model
        {
            public int Field { get; set; }
        }

        public class ModelMap : DocumentMap<Model>
        {
            public ModelMap()
            {
                Map(x => x.Field);
            }
        }
    }
}
