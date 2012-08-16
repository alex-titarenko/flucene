using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Odm.Mapping.Members;

namespace Lucene.Net.Odm.Mapping
{
    public class EmbeddedCollectionMapping : EmbeddedMapping
    {
        public EmbeddedCollectionMapping(Member member)
            : base(member)
        {
        }
    }
}
