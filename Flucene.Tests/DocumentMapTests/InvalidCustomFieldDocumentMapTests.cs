using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Odm.Mapping;
using NUnit.Framework;


namespace Lucene.Net.Orm.Tests.DocumentMapTests
{
    [TestFixture]
    public class InvalidCustomFieldDocumentMapTests
        : DocumentMapTestBase<InvalidCustomFieldDocumentMapTests.ModelMap, InvalidCustomFieldDocumentMapTests.Model>
    {
        [Test]
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
