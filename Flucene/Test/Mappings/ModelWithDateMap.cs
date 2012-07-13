using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Test.Models;

namespace Lucene.Net.Odm.Test.Mappings
{
    public class ModelWithDateMap: DocumentMap<ModelWithDate>
    {
        public ModelWithDateMap()
        {
            Map(x => x.DateField);
        }
    }
}
