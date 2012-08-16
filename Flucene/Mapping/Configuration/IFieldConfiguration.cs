using System;
using System.Collections.Generic;

using Lucene.Net.Documents;
using Lucene.Net.Analysis;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public interface IFieldConfiguration
    {
        string FieldName { get; }

        bool IsRequired { get; }


        IFieldConfiguration Analyze();

        IFieldConfiguration NoNormsAnalyze();

        IFieldConfiguration NotIndex();

        IFieldConfiguration NotAnalyze();

        IFieldConfiguration NoNormsNotAnalyze();


        IFieldConfiguration Store();

        IFieldConfiguration Compress();

        IFieldConfiguration NotStore();


        IFieldConfiguration Required();

        IFieldConfiguration Optional();


        IFieldConfiguration AnalyzerType<TAnalyzer>() where TAnalyzer : Analyzer;

        IFieldConfiguration AnalyzerType(Type analyzerType);


        IFieldConfiguration Boost(Boosting<object> boost);


        IEnumerable<Fieldable> GetFields(object value);
    }


    public interface IFieldConfiguration<TInput> : IFieldConfiguration
    {
        new IFieldConfiguration<TInput> Analyze();

        new IFieldConfiguration<TInput> NoNormsAnalyze();

        new IFieldConfiguration<TInput> NotIndex();

        new IFieldConfiguration<TInput> NotAnalyze();

        new IFieldConfiguration<TInput> NoNormsNotAnalyze();


        new IFieldConfiguration<TInput> Store();

        new IFieldConfiguration<TInput> Compress();

        new IFieldConfiguration<TInput> NotStore();


        new IFieldConfiguration<TInput> Required();

        new IFieldConfiguration<TInput> Optional();


        new IFieldConfiguration<TInput> AnalyzerType<TAnalyzer>() where TAnalyzer : Analyzer;

        new IFieldConfiguration<TInput> AnalyzerType(Type analyzerType);


        IFieldConfiguration<TInput> Boost(Boosting<TInput> boost);
    }
}
