using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Orm.Mapping;
using Lucene.Net.Orm.Test.Models;

namespace Lucene.Net.Orm.Test.Mappings
{
    public class ModelWithDateMap: DocumentMap<ModelWithDate>
    {
        public ModelWithDateMap()
        {
            Map(x => x.DateField);
        }
    }
}
