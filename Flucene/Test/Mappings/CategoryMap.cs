using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Odm.Mapping;
using Lucene.Net.Odm.Test.Models;


namespace Lucene.Net.Odm.Test.Mappings
{
    public class CategoryMap : DocumentMap<Category>
    {
        public CategoryMap()
        {
            //Map(x => x.ID);
            Map(x => x.Name, "ShopName").Store().Analyze().Boost(x => 1.5f);
            Map(x => x.IsRoot).Store().NotIndex();
        }
    }
}
