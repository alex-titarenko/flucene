using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Lucene.Net.Odm.Helpers
{
    /// <summary>
    /// Represents the extension methods for the lambda expression.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Converts the lambda expression to property info.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <typeparam name="TProperty">The type of model property.</typeparam>
        /// <param name="expression">Lambda expression that represents the property of the model.</param>
        /// <returns><see cref="System.Reflection.PropertyInfo"/> by specified <paramref name="expression"/>.</returns>
        public static PropertyInfo ToProperty<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException();

            Expression body = expression.Body;
            MemberExpression op = null;

            if (body is UnaryExpression)
                op = (body as UnaryExpression).Operand as MemberExpression;
            else if (body is MemberExpression)
                op = body as MemberExpression;

            PropertyInfo property = null;
            if (op != null)
            {
                MemberInfo member = op.Member;
                property = typeof(TModel).GetProperty(member.Name);

                if (property == null)
                {
                    throw new ArgumentException(Properties.Resources.EXC_INVALID_LAMBDA_EXPRESSION);
                }
            }

            return property;
        }

        /// <summary>
        /// Returns the property name by the specified lambda expression.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <typeparam name="TProperty">The type of model property.</typeparam>
        /// <param name="expression">Lambda expression that represents the property of the model.</param>
        /// <returns><see cref="System.String"/> represents the property name for the specified <paramref name="expression"/>.</returns>
        public static string GetPropertyName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
        {
            PropertyInfo info = ToProperty(expression);
            return (info != null) ? info.Name : null;
        }
    }
}
