using System;
using Lucene.Net.Odm.Mapping.Providers;
using Lucene.Net.Odm.Mapping.Members;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public class EmbeddedConfiguration : IEmbeddedMappingProvider
    {
        protected Member _member;
        protected string _prefix = null;

        public EmbeddedConfiguration(Member member)
        {
            _member = member;
        }


        public EmbeddedConfiguration Prefix(string prefix)
        {
            _prefix = prefix;
            return this;
        }


        #region IMappingProvider<EmbeddedMapping> Members

        public virtual EmbeddedMapping GetMapping()
        {
            EmbeddedMapping mapping = new EmbeddedMapping(_member);
            mapping.Prefix = _prefix;

            return mapping;
        }

        #endregion
    }
}
