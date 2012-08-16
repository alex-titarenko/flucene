using System;
using Lucene.Net.Documents;
using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Odm.Mapping
{
    public class FieldMapping : MemberMapping
    {
        public string FieldName { get; private set; }

        public float? Boost { get; set; }

        public Type AnalyzerType { get; set; }

        public bool IsRequired { get; set; }

        public bool IsNumeric { get; set; }

        public Field.Index Index = Field.Index.NOT_ANALYZED;

        public Field.Store Store = Field.Store.YES;


        public FieldMapping(string fieldName, Member member)
            : base(member)
        {
            IsRequired = true;
            FieldName = fieldName;
        }
    }
}
