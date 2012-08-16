using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Odm.Mapping.Members;
using Lucene.Net.Odm.Mapping.Providers;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public class EmbeddedCollectionConfiguration<TChild> : EmbeddedConfiguration
    {
        private Func<TChild, bool> _predicate;
        private int _itemCounts = int.MaxValue;

        public EmbeddedCollectionConfiguration(Member member)
            : base(member)
        {
        }


        public new EmbeddedCollectionConfiguration<TChild> Prefix(string prefix)
        {
            _prefix = prefix;
            return this;
        }

        public EmbeddedCollectionConfiguration<TChild> Count(int count)
        {
            _itemCounts = count;
            return this;
        }

        public EmbeddedCollectionConfiguration<TChild> Where(Func<TChild, bool> predicate)
        {
            _predicate = predicate;
            return this;
        }

        #region IEmbeddedMappingProvider Members

        public override EmbeddedMapping GetMapping()
        {
            EmbeddedCollectionMapping mapping =
                new EmbeddedCollectionMapping(new CollectionMember<TChild>(_member, _predicate, _itemCounts));
            mapping.Prefix = _prefix;
            return mapping;
        }

        #endregion
    }
}
