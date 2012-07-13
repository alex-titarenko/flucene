using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Orm
{
    public interface IIndexRepository<TModel>
    {
        void Add(TModel model);

        void Remove(TModel model);

        void Update(TModel model);

        void Commit();

        void Rollback();

        IEnumerable<TModel> Search(string query);
    }
}
