using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Orm.Test.Models
{
    public class Category
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsRoot { get; set; }
    }
}
