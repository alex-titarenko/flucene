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
    public class PropertyMapDocumentMapTests
        : DocumentMapTestBase<PropertyMapDocumentMapTests.ModelMap, PropertyMapDocumentMapTests.Model>
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
