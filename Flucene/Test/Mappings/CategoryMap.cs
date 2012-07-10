using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Orm.Mapping;
using Lucene.Net.Orm.Test.Models;


namespace Lucene.Net.Orm.Test.Mappings
{
    public class CategoryMap : DocumentMap<Category>
    {
        public CategoryMap()
        {
            //Map(x => x.ID);
            Map(x => x.Name, "ShopName").Store().Analyze().Boost(1.5f);
            Map(x => x.IsRoot).Store().NotIndex();
        }
    }
}
