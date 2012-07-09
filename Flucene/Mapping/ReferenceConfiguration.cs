using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Lucene.Net.Orm.Mapping
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
