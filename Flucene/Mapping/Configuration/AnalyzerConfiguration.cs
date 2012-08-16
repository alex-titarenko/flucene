using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public class AnalyzerConfiguration<T> where T : IHasAnalyzer
    {
        private readonly T _part;


        public AnalyzerConfiguration(T part)
        {
            _part = part;
        }


        public T Custom<TAnalyzer>()
        {
            _part.AnalyzerType = typeof(TAnalyzer);
            return _part;
        }

        public T Standard()
        {
            return Custom<StandardAnalyzer>();
        }

        public T Keyword()
        {
            return Custom<KeywordAnalyzer>();
        }

        public T Simple()
        {
            return Custom<SimpleAnalyzer>();
        }

        public T Stop()
        {
            return Custom<StopAnalyzer>();
        }

        public T Whitespace()
        {
            return Custom<WhitespaceAnalyzer>();
        }
    }
}
