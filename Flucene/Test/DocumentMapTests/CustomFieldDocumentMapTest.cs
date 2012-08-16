using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucene.Net.Odm.Mapping;

namespace Lucene.Net.Orm.Test.DocumentMapTests
{
    [TestClass]
    public class CustomFieldDocumentMapTest
        : DocumentMapTestBase<CustomFieldDocumentMapTest.ModelMap, CustomFieldDocumentMapTest.Model>
    {
        protected override void BuildExpectedModelMapping(DocumentMapping<Model> mapping)
        {
            mapping.Fields.Add(new FieldMapping("CustomField", GetCustomMember(null)));
        }


        public class Model
        {
            public string Text { get; set; }
        }

        public class ModelMap : DocumentMap<Model>
        {
            public ModelMap()
            {
                Map(x => x.Text.ToLowerInvariant(), "CustomField");
            }
        }
    }
}
