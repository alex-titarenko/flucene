using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Members;
using NUnit.Framework;


namespace Lucene.Net.Orm.Tests.Helpers
{
    public static class AssertHelper
    {
        public static void FieldAssert(FieldMapping expected, FieldMapping actual)
        {
            Assert.AreEqual(expected.FieldName, actual.FieldName);
            Assert.AreEqual(expected.Boost, actual.Boost);
            Assert.AreEqual(expected.AnalyzerType, actual.AnalyzerType);
            Assert.AreEqual(expected.IsNumeric, actual.IsNumeric);
            Assert.AreEqual(expected.IsRequired, actual.IsRequired);
            Assert.AreEqual(expected.Index, actual.Index);
            Assert.AreEqual(expected.Store, actual.Store);

            MemberAssert(expected.Member, actual.Member);
        }

        public static void EmbededAssert(EmbeddedMapping expected, EmbeddedMapping actual)
        {
            Assert.AreEqual(expected.Prefix, actual.Prefix);
            AssertHelper.MemberAssert(expected.Member, actual.Member);
        }

        public static void MemberAssert(Member expected, Member actual)
        {
            Assert.AreEqual(expected.CanWrite, actual.CanWrite);
            Assert.AreEqual(expected.MemberType, actual.MemberType);
        }
    }
}
