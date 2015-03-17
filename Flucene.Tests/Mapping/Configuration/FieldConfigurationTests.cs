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
        protected FieldConfiguration FieldConfiguration;
        protected FieldMapping ExpectedFieldMapping;


        [SetUp]
        public virtual void SetUp()
        {
            FieldConfiguration = new FieldConfiguration("FieldName", new MemberMock(true, typeof(string)));
            ExpectedFieldMapping = new FieldConfiguration("FieldName", new MemberMock(true, typeof(string))).GetMapping();
        }


        #region GetMapping

        [Test]
        public void GetMapping()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration;

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_AsNumeric()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration.AsNumeric();
            ExpectedFieldMapping.IsNumeric = true;

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_IsOptional()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration.Optional();
            ExpectedFieldMapping.IsRequired = false;

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_Index()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration.Index.NoNormsAnalyze();
            ExpectedFieldMapping.Index = Field.Index.ANALYZED_NO_NORMS;

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_Store()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration.Store.Yes();
            ExpectedFieldMapping.Store = Field.Store.YES;

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_PredefinedAnalyzer()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration.Analyzer.Keyword();
            ExpectedFieldMapping.AnalyzerType = typeof(KeywordAnalyzer);

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_CustomAnalyzer()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration.Analyzer.Custom<SimpleAnalyzer>();
            ExpectedFieldMapping.AnalyzerType = typeof(SimpleAnalyzer);

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_Boost()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration.Boost(2);
            ExpectedFieldMapping.Boost = 2;

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        [Test]
        public void GetMapping_Full()
        {
            //arrange
            FieldConfiguration field = FieldConfiguration
                .Analyzer.Stop()
                .AsNumeric()
                .Index.Analyze()
                .Store.No();

            ExpectedFieldMapping.IsNumeric = true;
            ExpectedFieldMapping.AnalyzerType = typeof(StopAnalyzer);
            ExpectedFieldMapping.Index = Field.Index.ANALYZED;
            ExpectedFieldMapping.Store = Field.Store.NO;

            //action
            FieldMapping actual = field.GetMapping();

            //assert
            AssertHelper.FieldAssert(ExpectedFieldMapping, actual);
        }

        #endregion
    }
}
