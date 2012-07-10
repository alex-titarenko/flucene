using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Orm.Test.Models
{
    public class Application
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        public virtual string Version { get; set; }

        public string Title
        {
            get
            {
                return Name + " " + Version;
            }
        }

        public virtual string Description { get; set; }

        public virtual Category Category { get; set; }

        public virtual decimal RegularPrice { get; set; }

        public virtual decimal? UpgradePrice { get; set; }

        public virtual DateTime ReleaseDate { get; set; }

        public virtual PublishStatus Status { get; set; }

        public virtual ICollection<string> Tags { get; set; }
    }
}
