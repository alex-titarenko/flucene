using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilesIndexer.Models
{
    public class MetaInfo
    {
        public virtual long Size { get; set; }
        public virtual bool Readonly { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime ModificationTime { get; set; }
    }
}
