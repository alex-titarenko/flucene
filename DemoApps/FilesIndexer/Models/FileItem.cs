using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilesIndexer.Models
{
    public class FileItem
    {
        public virtual string Filename { get; set; }
        public virtual IList<string> Text { get; set; }
        public virtual MetaInfo MetaInfo { get; set; }
    }
}
