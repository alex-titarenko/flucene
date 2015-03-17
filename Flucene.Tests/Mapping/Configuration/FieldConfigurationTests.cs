using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Odm.Mapping.Configuration;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Orm.Tests.DocumentMapTests;
using Lucene.Net.Orm.Tests.Helpers;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using NUnit.Framework;


namespace Lucene.Net.Orm.Tests.Mapping.Configuration
{
    [TestFixture]
    public class FieldConfigurationTests
    {
        private FieldConfiguration CreateFieldConfiguration()
        {
            return new FieldConfiguration("FieldName", new MemberMock(true, typeof(string)));
        }

        private FieldMapping CreateFieldMapping()
        {
            return CreateFieldConfiguration().GetMapping();
        }


        [Test]
        public void GetMappingTest()
        {
            FieldConfiguration field = CreateFieldConfiguration();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_AsNumeric()
        {
            FieldConfiguration field = CreateFieldConfiguration().AsNumeric();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.IsNumeric = true;

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_IsOptional()
        {
            FieldConfiguration field = CreateFieldConfiguration().Optional();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.IsRequired = false;

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_Index()
        {
            FieldConfiguration field = CreateFieldConfiguration().Index.NoNormsAnalyze();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.Index = Field.Index.ANALYZED_NO_NORMS;

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_Store()
        {
            FieldConfiguration field = CreateFieldConfiguration().Store.Compress();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.Store = Field.Store.COMPRESS;

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_PredefinedAnalyzer()
        {
            FieldConfiguration field = CreateFieldConfiguration().Analyzer.Keyword();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.AnalyzerType = typeof(KeywordAnalyzer);

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_CustomAnalyzer()
        {
            FieldConfiguration field = CreateFieldConfiguration().Analyzer.Custom<SimpleAnalyzer>();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.AnalyzerType = typeof(SimpleAnalyzer);

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_Boost()
        {
            FieldConfiguration field = CreateFieldConfiguration().Boost(2);

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.Boost = 2;

            AssertHelper.FieldAssert(expected, actual);
        }

        [Test]
        public void GetMappingTest_Full()
        {
            FieldConfiguration field = CreateFieldConfiguration()
                .Analyzer.Stop()
                .AsNumeric()
                .Index.Analyze()
                .Store.No();

            FieldMapping actual = field.GetMapping();
            FieldMapping expected = CreateFieldMapping();
            expected.IsNumeric = true;
            expected.AnalyzerType = typeof(StopAnalyzer);
            expected.Index = Field.Index.ANALYZED;
            expected.Store = Field.Store.NO;

            AssertHelper.FieldAssert(expected, actual);
        }
    }
}
