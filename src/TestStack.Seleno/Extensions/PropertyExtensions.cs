using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestStack.Seleno.Extensions
{
    public static class PropertyExtensions
    {
        public static TDomain SetValue<TDomain, TProperty>(this TDomain domainObject, Expression<Func<TDomain, TProperty>> domainPropertySelector, TProperty value)
            where TDomain : class
        {
            if (domainObject == null) return domainObject;

            var property = domainObject.GetPropertyFromLambda(domainPropertySelector);

            property.SetValue(domainObject, value, null);

            return domainObject;
        }

        public static PropertyInfo GetPropertyFromLambda<T, TProperty>(this Expression<Func<T, TProperty>> propertySelector)
            where T : class
        {
            var member = propertySelector.Body as MemberExpression;
            var propInfo = member.Member as PropertyInfo;
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
