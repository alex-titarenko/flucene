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
    public class CustomMapDocumentMapTests
        : DocumentMapTestBase<CustomMapDocumentMapTests.ModelMap, CustomMapDocumentMapTests.Model>
    {
        protected override void BuildExpectedModelMapping(DocumentMapping<Model> mapping)
        {
            mapping.Fields.Add(new FieldMapping("version", GetCustomMember(typeof(IEnumerable<string>))));
        }

        public class Model
        {
            public Version Version { get; set; }
        }

        public class ModelMap : DocumentMap<Model>
        {
            public ModelMap()
            {
                Map(x => x.Version.ToString(), (x, v) => x.Version = Version.Parse(v.First()), "version");
            }
        }
    }
}
