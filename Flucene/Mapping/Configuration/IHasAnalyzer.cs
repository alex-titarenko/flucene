using System;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public interface IHasAnalyzer
    {
        Type AnalyzerType { get; set; }
    }
}
