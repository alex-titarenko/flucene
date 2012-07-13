using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Reflection;
using Lucene.Net.Odm;
using Lucene.Net.Odm.Mappers;
using System.IO;
using FilesIndexer.Models;

namespace FilesIndexer
{
    /// <summary>
    /// Demo application. This application demonstrates basic principles of Object-Document Mapping (ODM).
    /// It is an analogue of ORM (Object relational mapping) for document-orientired database, like Lucene.
    /// </summary>
    class Program
    {
        // Limits for demo - we demonstrate principle only.
        /// <summary>
        /// Maximal files quantity for processing.
        /// </summary>
        const int MaxFilesCount = 1000;
        /// <summary>
        /// Maximal size of text in single file (in characters).
        /// </summary>
        const int MaxTextSize = 1024 * 1024;
        
        static IContainer _container;

        static void Main(string[] args)
        {
            Register();
            IndexFiles();
            SearchCycle();
        }

        private static void Register()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new Lucene.Net.Odm.FluentMappingsService(Assembly.GetExecutingAssembly()))
                .As<IMappingsService>().SingleInstance();

            _container = builder.Build();
        }

        private static void IndexFiles()
        {
            string pathForIndexing = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            pathForIndexing = Path.Combine(Path.GetDirectoryName(pathForIndexing), "Samples");
            IEnumerable<FileItem> itemsToIndexing = ItemsInPath(pathForIndexing);
        }

        private static IEnumerable<FileItem> ItemsInPath(string pathForIndexing)
        {
            throw new NotImplementedException();
        }

        private static void SearchCycle()
        {
            throw new NotImplementedException();
        }
    }
}
