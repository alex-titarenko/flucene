using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Odm;
using Lucene.Net.Odm.Mapping;


namespace Lucene.Net.Orm.Test
{
    public class MockMappingsService : IMappingsService
    {
        public IDictionary<Type, object> Mappings = new Dictionary<Type, object>();

        public Odm.Mappers.IDocumentMapper Mapper
        {
            get;
            set;
        }

        #region IMappingsService Members

        public Documents.Document GetDocument<T>(T model, string prefix = null) where T : new()
        {
            object mapping;
            if (Mappings.TryGetValue(model.GetType(), out mapping))
            {
                return Mapper.GetDocument((DocumentMapping<T>)mapping, model, this, prefix);
            }
            else
            {
                throw new Exception();
            }
        }

        public T GetModel<T>(Documents.Document doc) where T : new()
        {
            object mapping;
            if (Mappings.TryGetValue(typeof(T), out mapping))
            {
                return Mapper.GetModel<T>((DocumentMapping<T>)mapping, doc, this);
            }
            else
            {
                throw new Exception();
            }
        }

        public object GetModel(Documents.Document doc, Type modelType, string prefix = null)
        {
            dynamic mapping;
            if (Mappings.TryGetValue(modelType, out mapping))
            {
                return Mapper.GetModel(mapping, doc, this, prefix);
            }
            else
            {
                throw new Exception();
            }
        }

        #endregion
    }
}
