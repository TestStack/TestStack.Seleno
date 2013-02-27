using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestStack.Seleno.Extensions
{

    public class ExpressionPropertyVisitor : ExpressionVisitor
    {
        private readonly IList<PropertyInfo> _properties = new List<PropertyInfo>();
        public IEnumerable<PropertyInfo> GetPropertiesFrom(params Expression[] expressions)
        {
            Visit(new ReadOnlyCollection<Expression>(expressions));
            return _properties;
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            var property = node.Member as PropertyInfo;
            if (property != null && !_properties.Contains(property))
            {
                _properties.Add(property);
            }

            return base.VisitMember(node);
        }



    }
    public static class PropertyExtensions
    {
        public static bool CanWriteToProperty(this PropertyInfo property, object typedValue)
        {
            return typedValue != null && property.CanWrite;
        }

        public static TDomain SetValue<TDomain, TProperty>(this TDomain domainObject, Expression<Func<TDomain, TProperty>> domainPropertySelector, TProperty value)
            where TDomain : class
        {
            if (domainObject == null) return domainObject;

            var property = domainObject.GetPropertyFromLambda(domainPropertySelector);

            property.SetValue(domainObject, value, null);

            return domainObject;
        }

        public static PropertyInfo GetPropertyFromLambda(this LambdaExpression propertySelector)
        {
            var propInfo = new ExpressionPropertyVisitor().GetPropertiesFrom(propertySelector).FirstOrDefault();
            
            if (propInfo == null)
            {
                throw new InvalidOperationException
                    (String.Format("Expression '{0}' refers to a field, not a property.",
                                   propertySelector));
            }

            return propInfo;
        }

       
        public static PropertyInfo GetPropertyFromLambda<T, TProperty>(this T obj, Expression<Func<T, TProperty>> propertySelector)
            where T : class
        {
            if ((obj == null) || propertySelector == null)
            {
                return null;
            }

            return propertySelector.GetPropertyFromLambda();
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(this IEnumerable<Expression<Func<T, Object>>> expressions)
        {
            var arrayOfExpressions = Enumerable.Empty<Expression<Func<T, Object>>>().ToArray();
            if (expressions != null)
            {
                arrayOfExpressions = expressions.ToArray();
            }

            return new ExpressionPropertyVisitor().GetPropertiesFrom(arrayOfExpressions);
        }

        public static PropertyInfo FindPropertyMatching<T>(this T instance, Func<PropertyInfo, Boolean> propertyMatcher)
            where T : class
        {
            PropertyInfo result = null;
            if (instance != null)
            {
                result = instance.GetType().GetProperties().SingleOrDefault(propertyMatcher);
            }

            return result;
        }
    }
}
