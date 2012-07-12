using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Lucene.Net.Orm.Helpers
{
    public static class ExpressionHelper
    {
        public static PropertyInfo ExtractPropertyInfo<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
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
                throw new ArgumentException(Properties.Resources.EXC_INVALID_LAMBDA_EXPRESSION);
            }

            return property;
        }
    }
}
