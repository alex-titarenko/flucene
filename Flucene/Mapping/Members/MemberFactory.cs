using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

using Lucene.Net.Odm.Helpers;


namespace Lucene.Net.Odm.Mapping.Members
{
    public class MemberFactory : IMemberFactory
    {
        #region IMemberFactory Members

        public Member GetMember<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {   
            PropertyInfo property = expression.ToProperty();

            if (property != null)
            {
                return new PropertyMember(property);
            }
            else
            {
                return new CustomMember(expression.Compile(), null);
            }
        }

        #endregion
    }
}
