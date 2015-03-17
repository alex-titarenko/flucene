using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Odm.Mapping;
using NUnit.Framework;


namespace Lucene.Net.Orm.Tests.DocumentMapTests
{
    [TestFixture]
    public class CustomFieldDocumentMapTests
        : DocumentMapTestBase<CustomFieldDocumentMapTests.ModelMap, CustomFieldDocumentMapTests.Model>
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
