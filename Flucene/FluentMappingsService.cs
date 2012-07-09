using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Orm.Mapping;
using Lucene.Net.Orm.Mappers;


namespace Lucene.Net.Orm
{
    public class FluentMappingsService : IFluentMappingsService
    {
        private const string IDocumentMapperProviderInterfaceName = "IMappingProvider`1";

        public IDocumentMapper Mapper { get; set; }

        protected IDictionary<Type, object> Mappings { get; set; }


        public FluentMappingsService(Assembly assembly)
            : this(new Assembly[] { assembly })
        {
        }

        public FluentMappingsService(IEnumerable<Assembly> assemblies)
        {
            Mappings = new Dictionary<Type, object>();

            foreach (Assembly assembly in assemblies)
            {
                 IEnumerable<Type> mapTypes =
                     assembly.GetExportedTypes()
                     .Where(t => IsMappingProvider(t));

                 foreach (Type mapperType in mapTypes)
                 {
                     AddMapping(mapperType);
                 }
            }
        }

        public FluentMappingsService(Type type)
            : this(new Type[] { type })
        {
        }

        public FluentMappingsService(IEnumerable<Type> types)
        {
            Mappings = new Dictionary<Type, object>();

            foreach (Type mapType in types)
            {
                if (IsMappingProvider(mapType))
                    AddMapping(mapType);
                else
                    throw new ArgumentException(
                        String.Format("'{0}' type is not implement IDocumentMapper interface.", mapType));
            }
        }


        private void AddMapping(Type mapType)
        {
            Type modelType = mapType
                .GetInterface(IDocumentMapperProviderInterfaceName)
                .GetGenericArguments()[0];

            object mappingProvider = Activator.CreateInstance(mapType);
            object mapping = mapType.InvokeMember("GetMapping",
                BindingFlags.InvokeMethod, null, mappingProvider, null);

            Mappings.Add(new KeyValuePair<Type,object>(modelType, mapping));
        }

        private bool IsMappingProvider(Type type)
        {
            return type.GetInterface(IDocumentMapperProviderInterfaceName) != null;
        }


        #region IFluentMappingsService Members

        public Document GetDocument<T>(T model) where T : new()
        {
            object mapping;
            if (Mappings.TryGetValue(model.GetType(), out mapping))
            {
                return Mapper.GetDocument((DocumentMapping<T>)mapping, model, this);
            }

            throw new InvalidOperationException(
                String.Format(Properties.Resources.EXC_MAPPING_NOT_REGISTERED, typeof(T)));
        }

        public T GetModel<T>(Document doc) where T : new()
        {
            object mapping;
            if (Mappings.TryGetValue(typeof(T), out mapping))
            {
                return Mapper.GetModel((DocumentMapping<T>)mapping, doc, this);
            }

            throw new InvalidOperationException(
                String.Format(Properties.Resources.EXC_MAPPING_NOT_REGISTERED, typeof(T)));
        }

        public object GetModel(Document doc, Type modelType)
        {
            dynamic mapping;
            if (Mappings.TryGetValue(modelType, out mapping))
            {
                return Mapper.GetModel(mapping, doc, this);
            }

            throw new InvalidOperationException(
                String.Format(Properties.Resources.EXC_MAPPING_NOT_REGISTERED, modelType));
        }

        #endregion
    }
}
