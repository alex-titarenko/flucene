using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Mappers;


namespace Lucene.Net.Odm
{
    public class FluentMappingsService : IMappingsService
    {
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

                 foreach (Type mapType in mapTypes)
                 {
                     AddMapping(mapType);
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
                        String.Format(Properties.Resources.EXC_NOT_IMPL_DOCUMENT_MAPPER, mapType));
            }
        }


        private void AddMapping(Type mapType)
        {
            Type modelType = GetMappingProviderType(mapType).GetGenericArguments()[0];

            dynamic mappingProvider = Activator.CreateInstance(mapType);
            object mapping =  mappingProvider.GetMapping();

            Mappings.Add(new KeyValuePair<Type, object>(modelType, mapping));
        }

        private bool IsMappingProvider(Type type)
        {
            return GetMappingProviderType(type) != null;
        }

        private Type GetMappingProviderType(Type mapType)
        {
            return mapType
                .GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType &&
                    x.GetGenericTypeDefinition() == (typeof(IMappingProvider<>)));
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
