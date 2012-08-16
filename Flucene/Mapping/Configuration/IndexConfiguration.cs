using System;
using Lucene.Net.Documents;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public class IndexConfiguration<T> where T : IHasIndex
    {
        private readonly T _parent;


        public IndexConfiguration(T parent)
        {
            _parent = parent;
        }


        public T Analyze()
        {
            return AnalyzeMode(Field.Index.ANALYZED);
        }

        public T NoNormsAnalyze()
        {
            return AnalyzeMode(Field.Index.ANALYZED_NO_NORMS);
        }

        public T No()
        {
            return AnalyzeMode(Field.Index.NO);
        }

        public T NotAnalyze()
        {
            return AnalyzeMode(Field.Index.NOT_ANALYZED);
        }

        public T NoNormsNotAnalyze()
        {
            return AnalyzeMode(Field.Index.NOT_ANALYZED_NO_NORMS);
        }


        private T AnalyzeMode(Field.Index index)
        {
            _parent.Index = index;
            return _parent;
        }
    }
}
