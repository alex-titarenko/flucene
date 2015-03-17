using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Odm.Mapping;
using System.Reflection;
using Lucene.Net.Odm.Mapping.Members;
using Lucene.Net.Orm.Tests.Helpers;
using NUnit.Framework;


namespace Lucene.Net.Orm.Tests.DocumentMapTests
{
    [TestFixture]
    public abstract class DocumentMapTestBase<TMap, TModel>
        where TMap : DocumentMap<TModel>, new()
        where TModel : new() 
    {
        [Test]
        public virtual void GetMappingTest()
        {
            TMap map = new TMap();
            DocumentMapping<TModel> actual = map.GetMapping();

            DocumentMapping<TModel> expected = new DocumentMapping<TModel>();
            BuildExpectedModelMapping(expected);

            Test(expected, actual);
        }

        protected abstract void BuildExpectedModelMapping(DocumentMapping<TModel> mapping);


        private void Test(DocumentMapping<TModel> expected, DocumentMapping<TModel> actual)
        {
            Assert.AreEqual(expected.Fields.Count, actual.Fields.Count);

            IEnumerator<FieldMapping> expField, actField;
            for (expField = expected.Fields.GetEnumerator(), actField = actual.Fields.GetEnumerator(); expField.MoveNext() && actField.MoveNext();)
            {
                AssertHelper.FieldAssert(expField.Current, actField.Current);
            }

            Assert.AreEqual(expected.Embedded.Count, actual.Embedded.Count);

            IEnumerator<EmbeddedMapping> expEmb, actEmb;
            for (expEmb = expected.Embedded.GetEnumerator(), actEmb = actual.Embedded.GetEnumerator(); expEmb.MoveNext() && actEmb.MoveNext(); )
            {
                AssertHelper.EmbededAssert(expEmb.Current, actEmb.Current);
            }

            Assert.AreEqual(expected.CustomActions.Count, actual.CustomActions.Count);

            Assert.AreEqual(expected.Boost, actual.Boost);
        }

        #region Helpers

        protected PropertyInfo GetProperty(string name)
        {
            return typeof(TModel).GetProperty(name);
        }

        protected Member GetCustomMember(Type memberType)
        {
            return new MemberMock(memberType != null, memberType);
        }

        #endregion
    }
}
