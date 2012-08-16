using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lucene.Net.Odm;
using Lucene.Net.Odm.Mappers;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mapping.Members;
using Lucene.Net.Documents;


namespace Lucene.Net.Orm.Test
{
    [TestClass]
    public class ReflectionDocumentMapperTest
    {
        private IDocumentMapper _mapper = new ReflectionDocumentMapper();
        private DocumentMapping<TestModel> _mappingWithOptionalField;
        private DocumentMapping<TestModel> _mappingWithRequiredField;
        private DocumentMapping<TestModel> _mappingWithNumericField;

        private IMappingsService _mappingService;
        private IMappingsService _fullMappingService;

        [TestInitialize]
        public void Initialize()
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
                Mapper = _mapper,
                Mappings = new Dictionary<Type, object>() {
                    { typeof(TestSubModel), GetMappingBasedOnField<TestSubModel>(TestSubModel.IdFieldName) }
                }
            };

            DocumentMapping<TestSubModel> testSubModelMapping = GetMappingBasedOnField<TestSubModel>(TestSubModel.IdFieldName);
            testSubModelMapping.Embedded.Add(new EmbeddedMapping(new PropertyMember(typeof(TestSubModel).GetProperty(TestSubModel.SubSubModelFieldName))));

            _fullMappingService = new MockMappingsService()
            {
                Mapper = _mapper,
                Mappings = new Dictionary<Type, object>() {
                    { typeof(TestSubModel), testSubModelMapping },
                    { typeof(TestSubSubModel), GetMappingBasedOnField<TestSubSubModel>(TestSubModel.IdFieldName) }
                }
            };
        }


        [TestMethod]
        public void GetDocumentTest_OptionalField()
        {
            TestModel model = new TestModel();
            model.Text = null;

            Document doc = _mapper.GetDocument(_mappingWithOptionalField, model, null);
            Assert.AreEqual(1, doc.GetFields().Count);
            Assert.AreEqual(String.Empty, doc.Get(TestModel.TextFieldName));

            model.Text = "Some text";

            doc = _mapper.GetDocument(_mappingWithOptionalField, model, null);
            Assert.AreEqual(1, doc.GetFields().Count);
            Assert.AreEqual(model.Text, doc.Get(TestModel.TextFieldName));
        }

        [TestMethod]
        public void GetModelTest_OptionalField()
        {
            Document doc = new Document();

            TestModel model = _mapper.GetModel<TestModel>(_mappingWithOptionalField, doc, null);
            Assert.IsNull(model.Text);

            doc.Add(new Field(TestModel.TextFieldName, "Some text", Field.Store.YES, Field.Index.ANALYZED));

            model = _mapper.GetModel<TestModel>(_mappingWithOptionalField, doc, null);
            Assert.IsNotNull(model.Text);
        }


        [TestMethod]
        public void GetDocumentTest_RequiredField()
        {
            TestModel model = new TestModel();
            model.Text = "Some string";

            Document doc = _mapper.GetDocument(_mappingWithRequiredField, model, null);
            Assert.AreEqual(1, doc.GetFields().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDocumentTest_RequiredField_Fail()
        {
            TestModel model = new TestModel();
            model.Text = null;

            Document doc = _mapper.GetDocument(_mappingWithRequiredField, model, null);
            Assert.AreEqual(0, doc.GetFields().Count);
        }

        [TestMethod]
        public void GetModelTest_RequiredField()
        {
            Document doc = new Document();
            doc.Add(new Field(TestModel.TextFieldName, "Some text", Field.Store.YES, Field.Index.ANALYZED));

            TestModel model = _mapper.GetModel<TestModel>(_mappingWithRequiredField, doc, null);
            Assert.IsNotNull(model.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetModelTest_RequiredField_Fail()
        {
            Document doc = new Document();

            TestModel model = _mapper.GetModel<TestModel>(_mappingWithRequiredField, doc, null);
            Assert.IsNull(model.Text);
        }


        [TestMethod]
        public void GetDocumentTest_NumericField()
        {
            TestModel model = new TestModel();
            model.Numeric = 3.5;

            Document doc = _mapper.GetDocument(_mappingWithNumericField, model, null);

            Fieldable numericField = doc.GetFieldable(TestModel.NumericFieldName);
            Assert.IsInstanceOfType(numericField, typeof(NumericField));
        }

        [TestMethod]
        public void GetDocumentTest_NumericFieldAsText()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnField<TestModel>(TestModel.NumericFieldName);

            TestModel model = new TestModel();
            model.Numeric = 3.5;

            Document doc = _mapper.GetDocument(mapping, model, null);

            Fieldable numericField = doc.GetFieldable(TestModel.NumericFieldName);
            Assert.IsNotInstanceOfType(numericField, typeof(NumericField));
        }


        [TestMethod]
        public void GetModelTest_NumericField()
        {
            Document doc = new Document();
            NumericField numField = new NumericField(TestModel.NumericFieldName, Field.Store.YES, true);
            numField.SetDoubleValue(3.5);
            doc.Add(numField);

            TestModel model = _mapper.GetModel<TestModel>(_mappingWithNumericField, doc, null);
            Assert.AreEqual(3.5, model.Numeric);
        }

        [TestMethod]
        public void GetModelTest_NumericFieldAsText()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnField<TestModel>(TestModel.NumericFieldName);
            Document doc = new Document();
            doc.Add(new Field(TestModel.NumericFieldName, "3.5", Field.Store.YES, Field.Index.ANALYZED));

            TestModel model = _mapper.GetModel<TestModel>(mapping, doc, null);
            Assert.AreEqual(3.5, model.Numeric);
        }


        [TestMethod]
        public void GetDocumentTest_DefaultPrefixForEmbedded()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            TestModel model = new TestModel();
            model.SubModel = new TestSubModel() { ID = 5 };

            Document doc = _mapper.GetDocument(mapping, model, _mappingService);

            Assert.AreEqual(1, doc.GetFields().Count);
            Field field = doc.GetField(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName));
            Assert.IsNotNull(field);
        }

        [TestMethod]
        public void GetDocumentTest_CustomPrefixForEmbedded()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);
            EmbeddedMapping embeddedMapping = mapping.Embedded.First();
            embeddedMapping.Prefix = "CustomPrefix";

            TestModel model = new TestModel();
            model.SubModel = new TestSubModel() { ID = 5 };

            Document doc = _mapper.GetDocument(mapping, model, _mappingService);

            Assert.AreEqual(1, doc.GetFields().Count);
            Field field = doc.GetField(embeddedMapping.Prefix + TestSubModel.IdFieldName);
            Assert.IsNotNull(field);
        }


        [TestMethod]
        public void GetModelTest_DefaultPrefixForEmbedded()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            Document doc = new Document();
            doc.Add(new Field(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName),
                "5", Field.Store.YES, Field.Index.ANALYZED));

            TestModel model = _mapper.GetModel<TestModel>(mapping, doc, _mappingService);

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.SubModel);
            Assert.AreEqual(5, model.SubModel.ID);
        }

        [TestMethod]
        public void GetModelTest_CustomPrefixForEmbedded()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);
            EmbeddedMapping embeddedMapping = mapping.Embedded.First();
            embeddedMapping.Prefix = "CustomPrefix";

            Document doc = new Document();
            doc.Add(new Field(embeddedMapping.Prefix + TestSubModel.IdFieldName,
                "5", Field.Store.YES, Field.Index.ANALYZED));

            TestModel model = _mapper.GetModel<TestModel>(mapping, doc, _mappingService);

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.SubModel);
            Assert.AreEqual(5, model.SubModel.ID);
        }

        [TestMethod]
        public void GetDocumentTest_DefaultPrefixForSubEmbedded()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            TestModel model = new TestModel();
            model.SubModel = new TestSubModel() { ID = 5, SubSubModel = new TestSubSubModel() { ID = 9 } };

            Document doc = _mapper.GetDocument(mapping, model, _fullMappingService);

            Assert.AreEqual(2, doc.GetFields().Count);
            
            Field field1 = doc.GetField(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName));
            Assert.IsNotNull(field1);
            Assert.AreEqual("5", field1.StringValue());

            Field field2 = doc.GetField(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.SubSubModelFieldName, TestSubModel.IdFieldName));
            Assert.IsNotNull(field2);
            Assert.AreEqual("9", field2.StringValue());
        }

        [TestMethod]
        public void GetModelTest_DefaultPrefixForSubEmbedded()
        {
            DocumentMapping<TestModel> mapping = GetMappingBasedOnEmbedded<TestModel>(TestModel.SubModelFieldName);

            Document doc = new Document();
            doc.Add(new Field(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.IdFieldName),
                "5", Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(DefaultEmbeddedFieldName(TestModel.SubModelFieldName, TestSubModel.SubSubModelFieldName, TestSubModel.IdFieldName),
                "9", Field.Store.YES, Field.Index.ANALYZED));

            TestModel model = _mapper.GetModel<TestModel>(mapping, doc, _fullMappingService);

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.SubModel);
            Assert.AreEqual(5, model.SubModel.ID);
            Assert.IsNotNull(model.SubModel.SubSubModel);
            Assert.AreEqual(9, model.SubModel.SubSubModel.ID);
        }


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
    }
}
