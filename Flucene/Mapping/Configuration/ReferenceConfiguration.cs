using System;


namespace Lucene.Net.Odm.Mapping.Configuration
{
    public class ReferenceConfiguration : IReferenceConfiguration
    {
        protected string _prefix = String.Empty;


        #region IReferenceConfiguration Members

        public IReferenceConfiguration Prefix(string prefix)
        {
            _prefix = prefix;
            return this;
        }

        #endregion
    }
}
