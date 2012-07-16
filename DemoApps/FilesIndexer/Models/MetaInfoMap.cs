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
            Map(p => p.CreationTime).Analyze();
            Map(p => p.ModificationTime).Analyze();
            Map(p => p.Readonly);
            Map(p => p.Size).Analyze();
        }
    }
}
