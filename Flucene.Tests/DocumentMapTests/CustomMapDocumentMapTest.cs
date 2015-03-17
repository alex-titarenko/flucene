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
    public class CustomMapDocumentMapTest
        : DocumentMapTestBase<CustomMapDocumentMapTest.ModelMap, CustomMapDocumentMapTest.Model>
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
