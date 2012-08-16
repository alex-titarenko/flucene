using System;
using System.Collections.Generic;


namespace Lucene.Net.Odm
{
    public interface IIndexRepository<TModel> where TModel : class
    {
        void Add(TModel model);

        void Remove(TModel model);

        void Update(TModel model);

        void Commit();

        void Rollback();

        IEnumerable<TModel> Search(string query, int page, int count);
    }
}
