using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Members;
using NUnit.Framework;


namespace Lucene.Net.Orm.Tests.DocumentMapTests
{
    [TestFixture]
    public class PropertyCustomNameMapDocumentMapTests
        : DocumentMapTestBase<PropertyCustomNameMapDocumentMapTests.ModelMap, PropertyCustomNameMapDocumentMapTests.Model>
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
