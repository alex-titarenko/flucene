using System;

using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Odm.Mapping
{
    public class EmbeddedMapping : MemberMapping
    {
        public string Prefix { get; set; }

        public EmbeddedMapping(Member member)
            : base(member)
        {
        }
    }
}
