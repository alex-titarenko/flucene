using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lucene.Net.Odm.Mapping;


namespace Lucene.Net.Orm.Test.DocumentMapTests
{
    [TestClass]
    public class InvalidCustomFieldDocumentMapTest
        : DocumentMapTestBase<InvalidCustomFieldDocumentMapTest.ModelMap, InvalidCustomFieldDocumentMapTest.Model>
    {
        [TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        public override void GetMappingTest()
        {
            base.GetMappingTest();
        }

        protected override void BuildExpectedModelMapping(DocumentMapping<Model> mapping)
        {
            mapping.Fields.Add(new FieldMapping(null, GetCustomMember(null)));
        }

        public class Model
        {
            public string Title { get; set; }
        }

        public class ModelMap : DocumentMap<Model>
        {
            public ModelMap()
            {
                Map(x => x.Title.ToUpperInvariant());
            }
        }
    }
}
