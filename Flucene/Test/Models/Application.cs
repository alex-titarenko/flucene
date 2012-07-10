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


        public override bool Equals(object obj)
        {
            if (obj is Application)
                return Equals((Application)obj);
            return false;
        }

        public virtual bool Equals(Application obj)
        {
            if (ID != obj.ID) return false;
            if (Name != obj.Name) return false;
            if (Version != obj.Version) return false;
            if (Description != obj.Description) return false;

            if (!Category.Equals(Category, obj.Category)) return false;
            if (RegularPrice != obj.RegularPrice) return false;
            if (UpgradePrice != obj.UpgradePrice) return false;
            if (ReleaseDate != obj.ReleaseDate) return false;
            if (Status != obj.Status) return false;
            if (!Enumerable.SequenceEqual(Tags, obj.Tags)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
