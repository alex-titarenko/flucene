using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lucene.Net.Documents;

using Lucene.Net.Orm.Mappers;
using Lucene.Net.Orm.Test.Models;


namespace Lucene.Net.Orm.Test
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
            _mappingService.Mapper = new ReflectionDocumentMapper();
        }


        [TestMethod]
        public void TwoWayMappingTest()
        {
            Application expected = new Application();
            expected.ID = 2;
            expected.Name = "Kaspersky Internet Security 2012";
            expected.Description = "Kaspersky Internet Security 2012 delivers premium protection from viruses, Trojans, spam, hackers and more.";
            expected.Category = new Category() { IsRoot = true, Name = "Security" };
            expected.Version = new Version(11, 0, 0, 0);
            expected.ReleaseDate = DateTime.Today;
            expected.RegularPrice = 79.95m;
            expected.UpgradePrice = 59.95m;
            expected.Status = PublishStatus.Active;
            expected.Tags = new List<string>() { "antivirus", "security", "anti-spam", "kaspersky" };

            Document actualDoc = _mappingService.GetDocument(expected);
            Application actual = _mappingService.GetModel<Application>(actualDoc);

            Assert.AreEqual(expected, actual);
        }
    }
}
