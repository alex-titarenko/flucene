using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Orm.Mapping;

namespace FilesIndexer.Models
{
    public class MetaInfoMap : DocumentMap<MetaInfo>
    {
        public MetaInfoMap()
        {
            Map(p => p.CreationTime);
            Map(p => p.ModificationTime);
            Map(p => p.Readonly);
            Map(p => p.Size);
        }
    }
}
