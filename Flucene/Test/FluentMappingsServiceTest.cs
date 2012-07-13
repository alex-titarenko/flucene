using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lucene.Net.Documents;

using Lucene.Net.Odm.Mappers;
using Lucene.Net.Odm.Test.Models;
using System.Globalization;
using System.Threading;


namespace Lucene.Net.Odm.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FluentMappingsServiceTest
    {
        private IMappingsService _mappingService;

        
        public FluentMappingsServiceTest()
        {
            _mappingService = new FluentMappingsService(Assembly.GetExecutingAssembly());
            _mappingService.Mapper = new CompiledDocumentMapper();
            //_mappingService.Mapper = new ReflectionDocumentMapper();
        }


        [TestMethod]
        public void TwoWayMappingTest()
        {
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

            Document actualDoc = _mappingService.GetDocument(expected);
            Application actual = _mappingService.GetModel<Application>(actualDoc);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DifferentDateTimeLocalizationTest()
        {
            CultureInfo usersCulture = Thread.CurrentThread.CurrentCulture;
            
            ModelWithDate original = new ModelWithDate { DateField = new DateTime(2012, 7, 11) };

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us"); // MM/DD/YYYY
            Document doc = _mappingService.GetDocument(original);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-gb"); // DD/MM/YYYY
            ModelWithDate restored = _mappingService.GetModel<ModelWithDate>(doc);
            Thread.CurrentThread.CurrentCulture = usersCulture;

            Assert.AreEqual(original.DateField, restored.DateField);
        }

    }
}
