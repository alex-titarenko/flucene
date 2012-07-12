using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Reflection;
using Lucene.Net.Orm;
using Lucene.Net.Orm.Mappers;

namespace FilesIndexer
{
    class Program
    {
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

            builder.Register(c => new Lucene.Net.Orm.FluentMappingsService(Assembly.GetExecutingAssembly()))
                .As<IMappingsService>().SingleInstance();

            _container = builder.Build();
        }

        private static void IndexFiles()
        {
            throw new NotImplementedException();
        }

        private static void SearchCycle()
        {
            throw new NotImplementedException();
        }
    }
}
