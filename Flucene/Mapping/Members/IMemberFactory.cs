using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Lucene.Net.Odm.Mapping.Members
{
    public interface IMemberFactory
    {
        Member GetMember<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression);
    }
}
