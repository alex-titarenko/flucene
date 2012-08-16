using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Odm.Mapping;

namespace FilesIndexer.Models
{
    public class FileItemMap : DocumentMap<FileItem>
    {
        public FileItemMap()
        {
            Map(p => p.Filename).Index.Analyze().Boost(10);
            Embedded(p => p.MetaInfo);
            Map(p => p.Text).Index.Analyze();
        }
    }
}
