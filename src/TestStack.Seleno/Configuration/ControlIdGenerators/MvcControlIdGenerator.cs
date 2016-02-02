using System.Linq.Expressions;
using System.Web.Mvc;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.ControlIdGenerators
{
    /// <summary>
    /// Default control id generator - uses the same algorithm ASP.NET MVC uses to generate control ids.
    /// </summary>
    public class MvcControlIdGenerator : IControlIdGenerator
    {
        public string GetControlName(LambdaExpression expression)
        {
            if (expression.Body.NodeType == ExpressionType.Convert)
                expression = Expression.Lambda(((UnaryExpression) expression.Body).Operand);
            return ExpressionHelper.GetExpressionText(expression);
        }

        public string GetControlId(string name)
        {
            return Html401IdUtil.CreateSanitizedId(name);
        }
    }
}
