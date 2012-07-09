using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;


namespace Lucene.Net.Orm.Helpers
{
    public static class ExpressionHelper
    {
        public static PropertyInfo ExtractPropertyInfo<TModel>(Expression<Func<TModel, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException();

            Expression body = expression.Body;
            MemberExpression op = null;

            if (body is UnaryExpression)
                op = (body as UnaryExpression).Operand as MemberExpression;
            else if (body is MemberExpression)
                op = body as MemberExpression;

            MemberInfo member = op.Member;
            PropertyInfo property = typeof(TModel).GetProperty(member.Name);

            if (property == null)
            {
                throw new InvalidOperationException("Lambda expression is invalid.");
            }

            return property;
        }
    }
}
