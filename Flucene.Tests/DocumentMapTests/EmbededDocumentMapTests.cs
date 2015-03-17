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
    public class EmbededDocumentMapTests
        : DocumentMapTestBase<EmbededDocumentMapTests.ModelMap, EmbededDocumentMapTests.Model>
    {
        protected override void BuildExpectedModelMapping(DocumentMapping<Model> mapping)
        {
            mapping.Embedded.Add(new EmbeddedMapping(new PropertyMember(GetProperty("EmbededModel")))
            {
                Prefix = "Embeded_"
            });
        }


        public class Model
        {
            public SubModel EmbededModel { get; set; }
        }

        public class SubModel
        {
        }

        public class ModelMap : DocumentMap<Model>
        {
            public ModelMap()
            {
                Embedded(x => x.EmbededModel).Prefix("Embeded_");
            }
        }
    }
}
