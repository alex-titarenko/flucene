using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Odm.Mapping.Providers;
using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public class FieldConfiguration : IFieldMappingProvider, IHasStore, IHasIndex, IHasAnalyzer
    {
        private string _fieldName;
        private Member _member;

        private bool _isNumeric;
        private float? _boost;
        private bool _required = true;
        Field.Index IHasIndex.Index { get; set; }
        Field.Store IHasStore.Store { get; set; }
        Type IHasAnalyzer.AnalyzerType { get; set; }


        public FieldConfiguration(string fieldName, Member member)
        {
            ((IHasIndex)this).Index = Field.Index.NOT_ANALYZED;
            ((IHasStore)this).Store = Field.Store.YES;

            _fieldName = fieldName;
            _member = member;
        }


        #region IFieldConfiguration Members

        public IndexConfiguration<FieldConfiguration> Index
        {
            get
            {
                return new IndexConfiguration<FieldConfiguration>(this);
            }
        }

        public StoreConfiguration<FieldConfiguration> Store
        {
            get
            {
                return new StoreConfiguration<FieldConfiguration>(this);
            }
        }

        public AnalyzerConfiguration<FieldConfiguration> Analyzer
        {
            get
            {
                return new AnalyzerConfiguration<FieldConfiguration>(this);
            }
        }

        public FieldConfiguration Optional()
        {
            _required = false;
            return this;
        }

        public FieldConfiguration AsNumeric()
        {
            _isNumeric = true;
            return this;
        }

        public FieldConfiguration Boost(float? boost)
        {
            _boost = boost;
            return this;
        }

        #endregion

        #region IMappingProvider<FieldMapping> Members

        public FieldMapping GetMapping()
        {
            FieldMapping mapping = new FieldMapping(_fieldName, _member);
            mapping.Index = ((IHasIndex)this).Index;
            mapping.Store = ((IHasStore)this).Store;
            mapping.IsRequired = _required;
            mapping.AnalyzerType = ((IHasAnalyzer)this).AnalyzerType;
            mapping.Boost = _boost;
            mapping.IsNumeric = _isNumeric;

            return mapping;
        }

        #endregion
    }
}
