using System;
using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Odm.Mapping
{
    public class MemberMapping
    {
        public Member Member { get; private set; }


        public MemberMapping(Member member)
        {
            Member = member;
        }
    }
}
