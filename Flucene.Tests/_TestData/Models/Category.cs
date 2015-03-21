using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucene.Net.Odm.Test.TestData.Models
{
    public class Category
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsRoot { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is Category)
                return Equals((Category)obj);
            return false;
        }

        public virtual bool Equals(Category cat)
        {
            if (ID != cat.ID) return false;
            if (Name != cat.Name) return false;
            if (IsRoot != cat.IsRoot) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
