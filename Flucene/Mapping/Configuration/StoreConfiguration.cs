using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public class StoreConfiguration<T> where T : IHasStore
    {
        private readonly T _part;


        public StoreConfiguration(T part)
        {
            _part = part;
        }


        public T Yes()
        {
            return Store(Field.Store.YES);
        }

        public T No()
        {
            return Store(Field.Store.NO);
        }

        public T Compress()
        {
            return Store(Field.Store.COMPRESS);
        }


        private T Store(Field.Store store)
        {
            _part.Store = store;
            return _part;
        }
    }
}
