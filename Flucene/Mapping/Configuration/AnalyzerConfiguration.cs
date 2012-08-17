using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    /// <summary>
    /// Represents the analyzer configuration using fluent interface.
    /// </summary>
    /// <typeparam name="T">The type of part.</typeparam>
    public class AnalyzerConfiguration<T> where T : IHasAnalyzer
    {
        private readonly T _part;

        /// <summary>
        /// Initializes a new instance of the <see cref="Lucene.Net.Odm.Mapping.Configuration.AnalyzerConfiguration{T}" /> class.
        /// </summary>
        /// <param name="part">A part of fluent chain.</param>
        public AnalyzerConfiguration(T part)
        {
            _part = part;
        }


        /// <summary>
        /// Sets the custom analyzer.
        /// </summary>
        /// <typeparam name="TAnalyzer">The type of analyzer.</typeparam>
        /// <returns>part of fluent chain.</returns>
        public T Custom<TAnalyzer>()
        {
            _part.AnalyzerType = typeof(TAnalyzer);
            return _part;
        }

        /// <summary>
        /// Sets the standard analyzer.
        /// </summary>
        /// <returns>part of fluent chain.</returns>
        public T Standard()
        {
            return Custom<StandardAnalyzer>();
        }

        /// <summary>
        /// Sets the keyword analyzer.
        /// </summary>
        /// <returns>part of fluent chain.</returns>
        public T Keyword()
        {
            return Custom<KeywordAnalyzer>();
        }

        /// <summary>
        /// Sets the simple analyzer.
        /// </summary>
        /// <returns>part of fluent chain.</returns>
        public T Simple()
        {
            return Custom<SimpleAnalyzer>();
        }

        /// <summary>
        /// Sets the stop analyzer.
        /// </summary>
        /// <returns>part of fluent chain.</returns>
        public T Stop()
        {
            return Custom<StopAnalyzer>();
        }

        /// <summary>
        /// Sets the whitespace analyzer.
        /// </summary>
        /// <returns>part of fluent chain.</returns>
        public T Whitespace()
        {
            return Custom<WhitespaceAnalyzer>();
        }
    }
}
