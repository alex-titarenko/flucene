using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Odm.Mapping.Members;
using Lucene.Net.Odm.Mapping.Providers;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    /// <summary>
    /// Represents the embedded collection configuration using fluent interface.
    /// </summary>
    /// <typeparam name="TChild">The type of items in the collection.</typeparam>
    public class EmbeddedCollectionConfiguration<TChild> : EmbeddedConfiguration
    {
        private Func<TChild, bool> _predicate;
        private int _itemCounts = int.MaxValue;


        /// <summary>
        /// Initializes a new instance of the <see cref="Lucene.Net.Odm.Mapping.Configuration.EmbeddedCollectionConfiguration{T}" /> class.
        /// </summary>
        /// <param name="member">A member for mapping.</param>
        public EmbeddedCollectionConfiguration(Member member)
            : base(member)
        {
        }


        /// <summary>
        /// Sets the prefix for all fied names.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
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
