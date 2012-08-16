using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Odm.Mapping;

namespace FilesIndexer.Models
{
    public class MetaInfoMap : DocumentMap<MetaInfo>
    {
        public MetaInfoMap()
        {
            Map(p => p.CreationTime).Index.Analyze();
            Map(p => p.ModificationTime).Index.Analyze();
            Map(p => p.Readonly);
            Map(p => p.Size).Index.Analyze();
        }
    }
}
