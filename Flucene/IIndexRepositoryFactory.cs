using System;


namespace Lucene.Net.Odm
{
    public interface IIndexRepositoryFactory
    {
        IIndexRepository<TModel> GetRepository<TModel>() where TModel : class;
    }
}
