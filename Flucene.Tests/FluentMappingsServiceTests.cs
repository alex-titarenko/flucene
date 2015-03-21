using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Lucene.Net.Documents;
using Lucene.Net.Odm.Mappers;
using Lucene.Net.Odm.Test.TestData.Models;
using System.Globalization;
using System.Threading;
using NUnit.Framework;


namespace Lucene.Net.Odm.Tests
{
    [TestFixture]
    public class FluentMappingsServiceTest
    {
        protected FluentMappingsService Target;

        
        [SetUp]
        public void SetUp()
        {
            Target = new FluentMappingsService(Assembly.GetExecutingAssembly())
            {
                Mapper = new ReflectionDocumentMapper()
            };
        }


        [Test]
        public void TwoWayMappingTest()
        {
            //arrange
            Application expected = new Application();
            expected.ID = 2;
            expected.Name = "Flucene";
            expected.Description = 
@"Flucene - ORM for lucene.net. This project based on the opened sources of Iveonik Systems ltd. 
This library provide more fine mapping as compared with other existed libraries. Library's name 
was created as acronym of ""FLUent luCENE"".";
            expected.Category = new Category() { IsRoot = true, Name = "Security" };
            expected.Version = new Version(1, 0, 0, 0);
            expected.ReleaseDate = DateTime.Today;
            expected.RegularPrice = 0.00m;
            expected.UpgradePrice = 59.95m;
            expected.Status = PublishStatus.Active;
            expected.Tags = new List<string>() { "lucene", "search", "ORM", "mapping" };
            expected.AdditionalFields = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Publisher", "Iveonik Systems Ltd."),
                new KeyValuePair<string, string>("License", "Apache License 2.0"),
                new KeyValuePair<string, string>("Language", "English")
            };

            //action
            Document actualDoc = Target.GetDocument(expected);
            Application actual = Target.GetModel<Application>(actualDoc);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DifferentDateTimeLocalizationTest()
        {
            //arrange
            CultureInfo usersCulture = Thread.CurrentThread.CurrentCulture;
            
            ModelWithDate original = new ModelWithDate { DateField = new DateTime(2012, 7, 11) };

            //action
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us"); // MM/DD/YYYY
            Document doc = Target.GetDocument(original);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-gb"); // DD/MM/YYYY
            ModelWithDate restored = Target.GetModel<ModelWithDate>(doc);
            Thread.CurrentThread.CurrentCulture = usersCulture;

            //assert
            Assert.AreEqual(original.DateField, restored.DateField);
        }

    }
}
