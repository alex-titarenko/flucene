using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Odm;
using Lucene.Net.Odm.Mappers;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Members;
using Lucene.Net.Documents;
using NUnit.Framework;


namespace Lucene.Net.Orm.Tests.Mappers
{
    [TestFixture]
    public class ReflectionDocumentMapperTests
    {
        protected IDocumentMapper Target = new ReflectionDocumentMapper();
        private DocumentMapping<TestModel> _mappingWithOptionalField;
        private DocumentMapping<TestModel> _mappingWithRequiredField;
        private DocumentMapping<TestModel> _mappingWithNumericField;

        private IMappingsService _mappingService;
        private IMappingsService _fullMappingService;


        [SetUp]
        public void SetUp()
        {
            _mappingWithOptionalField = GetMappingBasedOnField<TestModel>(TestModel.TextFieldName);
            FieldMapping fieldMapping = _mappingWithOptionalField.Fields.First();
            fieldMapping.IsRequired = false;

            _mappingWithRequiredField = GetMappingBasedOnField<TestModel>(TestModel.TextFieldName);
            fieldMapping = _mappingWithRequiredField.Fields.First();
            fieldMapping.IsRequired = true;

            _mappingWithNumericField = GetMappingBasedOnField<TestModel>(TestModel.NumericFieldName);
            fieldMapping = _mappingWithNumericField.Fields.First();
            fieldMapping.IsNumeric = true;


            _mappingService = new MockMappingsService()
            {
                Mapper = Target,
                Mappings = new Dictionary<Type, object>() {
                    { typeof(TestSubModel), GetMappingBasedOnField<TestSubModel>(TestSubModel.IdFieldName) }
                }
            };

            DocumentMapping<TestSubModel> testSubModelMapping = GetMappingBasedOnField<TestSubModel>(TestSubModel.IdFieldName);
            testSubModelMapping.Embedded.Add(new EmbeddedMapping(new PropertyMember(typeof(TestSubModel).GetProperty(TestSubModel.SubSubModelFieldName))));

            _fullMappingService = new MockMappingsService()
            {
                Mapper = Target,
                Mappings = new Dictionary<Type, object>() {
                    { typeof(TestSubModel), testSubModelMapping },
                    { typeof(TestSubSubModel), GetMappingBasedOnField<TestSubSubModel>(TestSubModel.IdFieldName) }
                }
            };
        }


        #region GetDocument

        [Test]
        public void GetDocument_OptionalField()
        {
            //arrange
            TestModel model = new TestModel();
            model.Text = null;

            Document doc = Target.GetDocument(_mappingWithOptionalField, model, null);
            Assert.AreEqual(1, doc.GetFields().Count);
            Assert.AreEqual(String.Empty, doc.Get(TestModel.TextFieldName));

            model.Text = "Some text";

            //action
            doc = Target.GetDocument(_mappingWithOptionalField, model, null);

            //assert
            Assert.AreEqual(1, doc.GetFields().Count);
            Assert.AreEqual(model.Text, doc.Get(TestModel.TextFieldName));
        }

        [Test]
        public void GetDocument_RequiredField()
        {
            //arrange
            TestModel model = new TestModel();
            model.Text = "Some string";

            //action
            Document doc = Target.GetDocument(_mappingWithRequiredField, model, null);

            //assert
            Assert.AreEqual(1, doc.GetFields().Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDocument_MissedRequiredField_ThrowArgumentNullException()
        {
            //arrange
            TestModel model = new TestModel();
            model.Text = null;

            //action
            Document doc = Target.GetDocument(_mappingWithRequiredField, model, null);
        }

        [Test]
        public void GetDocument_NumericField()
        {
            //arrange
            TestModel model = new TestModel();
            model.Numeric = 3.5;

            //action
            Document doc = Target.GetDocument(_mappingWithNumericField, model, null);
            var numericField = doc.GetFieldable(TestModel.NumericFieldName);

            //assert
            Assert.IsInstanceOf<NumericField>(numericField);
        }

        [Test]
        public void GetDocument_NumericFieldAsText()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnField<TestModel>(TestModel.NumericFieldName);

            TestModel model = new TestModel();
            model.Numeric = 3.5;

            //action
            Document doc = Target.GetDocument(mapping, model, null);
            var numericField = doc.GetFieldable(TestModel.NumericFieldName);

            //assert
            Assert.IsNotInstanceOf<NumericField>(numericField);
        }

        [Test]
        public void GetDocument_DefaultPrefixForEmbedded()
        {
            //arrange
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            TestModel model = new TestModel();
            model.SubModel = new TestSubModel() { ID = 5 };

            //action
            Document doc = Target.GetDocument(mapping, model, _mappingService);

            //assert
            Assert.AreEqual(1, doc.GetFields().Count);
            Field field = doc.GetField(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName));
            Assert.IsNotNull(field);
        }

        [Test]
        public void GetDocument_CustomPrefixForEmbedded()
        {
            //arrange
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);
            EmbeddedMapping embeddedMapping = mapping.Embedded.First();
            embeddedMapping.Prefix = "CustomPrefix";

            TestModel model = new TestModel();
            model.SubModel = new TestSubModel() { ID = 5 };

            //action
            Document doc = Target.GetDocument(mapping, model, _mappingService);

            //assert
            Assert.AreEqual(1, doc.GetFields().Count);
            Field field = doc.GetField(embeddedMapping.Prefix + TestSubModel.IdFieldName);
            Assert.IsNotNull(field);
        }

        [Test]
        public void GetDocument_DefaultPrefixForSubEmbedded()
        {
            //arrange
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            TestModel model = new TestModel();
            model.SubModel = new TestSubModel() { ID = 5, SubSubModel = new TestSubSubModel() { ID = 9 } };

            //action
            Document doc = Target.GetDocument(mapping, model, _fullMappingService);

            //assert
            Assert.AreEqual(2, doc.GetFields().Count);

            Field field1 = doc.GetField(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName));
            Assert.IsNotNull(field1);
            Assert.AreEqual("5", field1.StringValue);

            Field field2 = doc.GetField(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.SubSubModelFieldName, TestSubModel.IdFieldName));
            Assert.IsNotNull(field2);
            Assert.AreEqual("9", field2.StringValue);
        }

        #endregion

        #region GetModel

        [Test]
        public void GetModel_OptionalFieldMissed_Null()
        {
            //arrange
            Document doc = new Document();

            //action
            TestModel model = Target.GetModel<TestModel>(_mappingWithOptionalField, doc, null);
            
            //assert
            Assert.IsNull(model.Text);
        }

        [Test]
        public void GetModel_OptionalFieldPresent_NotNull()
        {
            //arrange
            var doc = new Document();
            doc.Add(new Field(TestModel.TextFieldName, "Some text", Field.Store.YES, Field.Index.ANALYZED));

            //action
            var model = Target.GetModel<TestModel>(_mappingWithOptionalField, doc, null);

            //assert
            Assert.IsNotNull(model.Text);
        }

        [Test]
        public void GetModel_RequiredFieldPresent_NotNull()
        {
            //arrange
            Document doc = new Document();
            doc.Add(new Field(TestModel.TextFieldName, "Some text", Field.Store.YES, Field.Index.ANALYZED));

            //action
            TestModel model = Target.GetModel<TestModel>(_mappingWithRequiredField, doc, null);
            
            //assert
            Assert.IsNotNull(model.Text);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetModel_RequiredFieldMissed_ThrowArgumentException()
        {
            //arrange
            Document doc = new Document();

            //action
            Target.GetModel<TestModel>(_mappingWithRequiredField, doc, null);
        }

        [Test]
        public void GetModel_NumericField_DoubleValue()
        {
            //arrange
            Document doc = new Document();
            NumericField numField = new NumericField(TestModel.NumericFieldName, Field.Store.YES, true);
            numField.SetDoubleValue(3.5);
            doc.Add(numField);

            //action
            TestModel model = Target.GetModel<TestModel>(_mappingWithNumericField, doc, null);
            
            //assert
            Assert.AreEqual(3.5, model.Numeric);
        }

        [Test]
        public void GetModel_NumericFieldAsText_DoubleValue()
        {
            //arrange
            DocumentMapping<TestModel> mapping = GetMappingBasedOnField<TestModel>(TestModel.NumericFieldName);
            Document doc = new Document();
            doc.Add(new Field(TestModel.NumericFieldName, "3.5", Field.Store.YES, Field.Index.ANALYZED));

            //action
            TestModel model = Target.GetModel<TestModel>(mapping, doc, null);
            
            //assert
            Assert.AreEqual(3.5, model.Numeric);
        }

        [Test]
        public void GetModel_DefaultPrefixForEmbedded()
        {
            //arrange
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            Document doc = new Document();
            doc.Add(new Field(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName),
                "5", Field.Store.YES, Field.Index.ANALYZED));

            //action
            TestModel model = Target.GetModel<TestModel>(mapping, doc, _mappingService);

            //assert
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.SubModel);
            Assert.AreEqual(5, model.SubModel.ID);
        }

        [Test]
        public void GetModel_CustomPrefixForEmbedded()
        {
            //arrange
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);
            EmbeddedMapping embeddedMapping = mapping.Embedded.First();
            embeddedMapping.Prefix = "CustomPrefix";

            Document doc = new Document();
            doc.Add(new Field(embeddedMapping.Prefix + TestSubModel.IdFieldName,
                "5", Field.Store.YES, Field.Index.ANALYZED));

            //action
            TestModel model = Target.GetModel<TestModel>(mapping, doc, _mappingService);

            //assert
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.SubModel);
            Assert.AreEqual(5, model.SubModel.ID);
        }

        [Test]
        public void GetModel_DefaultPrefixForSubEmbedded()
        {
            //arrange
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            Document doc = new Document();
            doc.Add(new Field(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName),
                "5", Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.SubSubModelFieldName, TestSubModel.IdFieldName),
                "9", Field.Store.YES, Field.Index.ANALYZED));

            //action
            TestModel model = Target.GetModel<TestModel>(mapping, doc, _fullMappingService);

            //assert
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.SubModel);
            Assert.AreEqual(5, model.SubModel.ID);
            Assert.IsNotNull(model.SubModel.SubSubModel);
            Assert.AreEqual(9, model.SubModel.SubSubModel.ID);
        }

        #endregion


        #region Helpers

        private static string DefaultEmbeddedFieldName(params string[] parts)
        {
            return String.Join(".", parts);
        }

        private static DocumentMapping<TModel> GetMappingBasedOnField<TModel>(string fieldName)
        {
            DocumentMapping<TModel> mapping = new DocumentMapping<TModel>();
            mapping.Fields.Add(new FieldMapping(fieldName,
                new PropertyMember(typeof(TModel).GetProperty(fieldName))));

            return mapping;
        }

        private static DocumentMapping<TModel> GetMappingBasedOnEmbedded<TModel>(string fieldName)
        {
            DocumentMapping<TModel> mapping = new DocumentMapping<TModel>();
            mapping.Embedded.Add(new EmbeddedMapping(new PropertyMember(typeof(TModel).GetProperty(fieldName))));

            return mapping;
        }

        #endregion

        #region Tests Data

        public class TestModel
        {
            internal const string TextFieldName = "Text";
            internal const string NumericFieldName = "Numeric";
            internal const string SubModelFieldName = "SubModel";

            public string Text { get; set; }

            public double Numeric { get; set; }

            public TestSubModel SubModel { get; set; }
        }

        public class TestSubModel
        {
            internal const string IdFieldName = "ID";
            internal const string SubSubModelFieldName = "SubSubModel";

            public int ID { get; set; }

            public TestSubSubModel SubSubModel { get; set; }
        }

        public class TestSubSubModel
        {
            public int ID { get; set; }
        }

        #endregion
    }
}
